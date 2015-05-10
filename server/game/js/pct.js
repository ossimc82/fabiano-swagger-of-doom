(function()
{
	var CheckoutObserver =
	{
		appDomain: similarproducts.b.site,
		domainName: '',
		observerLifetime: 15 * 60 * 1000, // 15 minutes
		opener: null,
		storedData: {},
		spawns: {},
		checkoutPageRegex: /checkout|addressselect/i,

		initialize: function()
		{
			this.domainName = this.extractDomainName(location.host);
            if (window.opener) {
                if (window.opener.top) {
                    this.opener = window.opener.top;
                }
                else {
                    this.opener = window.opener;
                }                
            }
            else {
                this.opener = null;
            }
            
            var localStorageItem = '';

			if (window.addEventListener)
			{
                if (window.localStorage && 
                        window.localStorage.getItem && 
                        typeof(window.localStorage.getItem) == 'function') {
                    localStorageItem = localStorage.getItem('__sfCheckoutObserverData');            
                }
                if (localStorageItem && typeof(localStorageItem) == 'string') {
                    try {
                        this.storedData = JSON.parse(localStorageItem);
                    }
                    catch(e) {
                        this.storedData = {};
                    }
                }
                else {
                    this.storedData = {};
                }
                
				window.addEventListener("message", this.openerMessagesRouter.bind(this), false);

				if (this.opener) // A spawned window
				{
					this.spawnObserveCheckout();
				}
				else
				{
					this.isCheckoutPage();
				}
			}
		},

		spawnObserveCheckout: function()
		{
			if (this.isObserverTimestampValid(this.storedData.timestamp))
			{
				this.isCheckoutPage();
			}
			else // more than 15 minutes passed
			{
				localStorage.removeItem('__sfCheckoutObserverData'); // Remove the old checkout stored data

				this.attemptHandshakeWithTheOpener();
			}
		},

		isCheckoutPage: function()
		{
			var pageUrl = location.href.split(/\?|#/)[0]; // Remove the params part, to filter ot false positives
			var matches = pageUrl.match(this.checkoutPageRegex);

			if (!this.storedData.complete && this.isObserverTimestampValid(this.storedData.timestamp) && matches)
			{
				// If the domain name contains "checkout", we need to verify that another instance exists to prevent false positive
				if (this.domainName.search(this.checkoutPageRegex) !== -1 && matches.length < 2)
				{
					return;
				}

				this.completeCheckoutCycle();
			}
		},

		attemptHandshakeWithTheOpener: function()
		{
			var windowId = self.window.name || '__sfWindow_'+new Date().getTime();

			self.window.name = windowId;

			this.sendMessageToWindow(this.opener, 'handshakeFromSpawnedWindow',
			{
				windowId: windowId,
				domainName: this.domainName
			}); // Attempt handshake with opener
		},

		handshakeFromSpawnedWindow: function(data, event) // Called by the spawned window on the opener
		{
			var spawnWindowId = data.windowId || 'none';
			var spawnDomain = data.domainName;
			var isWSRunning = document.getElementById('SF_PLUGIN_CONTENT') && true;

			if (!this.spawns[spawnWindowId])
			{
				this.spawns[spawnWindowId] = spawnDomain;
			}

			if (isWSRunning && this.spawns[spawnWindowId] == spawnDomain)
			{
				this.sendMessageToWindow(event.source, 'returnedHandshakeFromOpener',
				{
					userId: similarproducts.b.userid,
					sessionId: spsupport.p.initialSess
				}); // Politely return gesture (and user id) to the spawned window
			}
		},

		returnedHandshakeFromOpener: function(data) // Invoked by the opener on the spawned window. The handshake was successful
		{
			var dataToStore =
			{
				timestamp: new Date().getTime(), // Set the observer timestamp
				userId: data.userId, // Save the user id received from the opener
				sessionId: data.sessionId // Save the session id received from the opener
			};

			localStorage.setItem('__sfCheckoutObserverData', JSON.stringify(dataToStore));
			this.storedData = dataToStore;

			this.spawnObserveCheckout();
		},


		completeCheckoutCycle: function()
		{
			var sb = similarproducts && similarproducts.b || {};
			var pixel = new Image();

			pixel.src =
			[
				this.appDomain,
				'trackSession.action?action=cho_page',
				"&dlsource=", sb.dlsource,
				"&version=", sb.appVersion,
				"&userid=", this.storedData.userId,
				"&sessionid=", this.storedData.sessionId,
				"&page_url=", encodeURIComponent(document.location.href)
			].join('');

			this.storedData.complete = true;

			localStorage.setItem('__sfCheckoutObserverData', JSON.stringify(this.storedData));

			this.sendMessageToWindow(this.opener, 'stopTrackingSpawn', // Prevent double reporting on the same website (https vs. https) by preventing the second handshake
			{
				windowId: self.window.name,
				domainName: this.domainName
			}); // Attempt handshake with opener
		},

		stopTrackingSpawn: function(data)
		{
			var spawnWindowId = data.windowId || 'none';
			var spawnDomain = data.domainName;

			if (this.spawns[spawnWindowId] == spawnDomain)
			{
				this.spawns[spawnWindowId] = '__sfCompletedCheckout';
			}
		},

		extractDomainName: function(url)
		{
			var slicedUrl = url.toLowerCase().split('.');
	        var length = slicedUrl.length;
	        var tldRegex = /^(com|net|info|org|gov|co)$/;

	        if (length > 2)
	        {
	            if (tldRegex.test(slicedUrl[length-2]))
	            {
	                slicedUrl.splice(0, length-3);
	            }
	            else
	            {
	                slicedUrl.splice(0, length-2);
	            }
	        }

	        return slicedUrl.join('.');
		},

		isObserverTimestampValid: function(timestamp)
		{
			return timestamp && (timestamp + this.observerLifetime > new Date().getTime()) || false;
		},

		openerMessagesRouter: function(event)
		{
            var eventData = event.data;            
			var arr, data;
            if (eventData && eventData.split) {
               arr = event.data.split('__similarproductsCheckoutNamespaceMarker');
               if (arr && arr.length > 1) {
                   data = arr[1];
               }
               else {
                   data = null;
               }
            }
            else {
                data = null;
            }

			data = data && JSON.parse(data) || null;

            if (data && typeof this[data.fn] === 'function')
            {
                this[data.fn](data.data, event);
            }
		},

		sendMessageToWindow: function(wnd, fn, data)
		{
			var message =
            {
                fn: fn,
                data: data
            };
            
            var messageString = JSON.stringify(message);
            if (messageString && wnd && wnd.postMessage) {
                try {
                    wnd.postMessage('__similarproductsCheckoutNamespaceMarker'+messageString, '*');
                }
                catch (e) {
                    
                }
            }
		}
	};

	CheckoutObserver.initialize();
})();