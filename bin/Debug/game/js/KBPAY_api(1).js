var JSON = JSON || {};
JSON.stringify = JSON.stringify || function (obj) {
	var t = typeof (obj);  
	if (t != "object" || obj === null) {  
		// simple data type  
		if (t == "string") obj = '"'+obj+'"';  
		return String(obj);  
	}  
	else {  
		// recurse array or object  
		var n, v, json = [], arr = (obj && obj.constructor == Array);  
		for (n in obj) {  
			v = obj[n]; t = typeof(v);  
			if (t == "string") v = '"'+v+'"';  
			else if (t == "object" && v !== null) v = JSON.stringify(v);  
			json.push((arr ? "" : '"' + n + '":') + String(v));  
		}  
		return (arr ? "[" : "{") + String(json) + (arr ? "]" : "}");  
	}  
};
		
var KBPAY = new function()
{
	var _options;
	var _self = this;
	var _shop;
	var _onPaymentProviderClose;
	
	/**
	 * Utility class to make inheritance a little bit simpler
	 */
	var _OOP = new function()
	{
		/**
		 * set a Child Class to be a subclass of Parent Class
		 * @static
		 * @param {Function} childClass The child class
		 * @param {Function} parentClass The parent class
		 */
		this.inherits = function(childClass, parentClass)
		{
			if(typeof(childClass) != "function" || typeof(parentClass) != "function")
			{
				throw "Illegal Arguments: both child class and parent class must be a function.";
			}
			childClass.prototype = new parentClass();
			childClass.prototype.constructor = parentClass;
		};
	}();


	/**
	 * Create a Event Dispatcher, 
	 * Can be instantiated directly but it is preferred to create your own class
	 * and extend this class so you objects can trigger events of their own
	 * @class represents a event disptacher 
	 * @constructor
	 * @param {string} eventType type of event
	 */
	var _CustomEventDispatcher = function () {

		var _eventListeners = {};

		var _getListenerPosition = function (eventType, newCallback) {
			var position = -1;
			if (_eventListeners[eventType]) {
				var i;
				var callbackCount = _eventListeners[eventType].length;
				var callbacks = _eventListeners[eventType];
				for (i = 0; i < callbackCount; i++) {
					var callback = callbacks[i];
					if (callback == newCallback) {
						position = i;
						break;
					}
				}
			}
			return position;
		};
		
		/**
		 * Listen to a custom events
		 * @param {String} eventType Event type
		 * @param {Function} callback Callback function
		 */
		this.addEventListener = function (eventType, callback) {
			if (typeof (callback) !== "function") {
				// callback is not a function, do nothing
				return;
			}
			if (_eventListeners[eventType]) {
				if (_getListenerPosition(eventType, callback) < 0) {
					_eventListeners[eventType].push(callback);
				}
			}
			else {
				_eventListeners[eventType] = [callback];
			}

		};

		/**
		 * Stop listening to a custom events
		 * @param {String} eventType Event type
		 * @param {Function} callback Callback function
		 */
		this.removeEventListener = function (eventType, callback) {
			var position = _getListenerPosition(eventType, callback);
			if (position >= 0) {
				// found the listener,remove it
				var callbacks = _eventListeners[eventType];
				callbacks.splice(position, 1);
			}
		};

		/**
		 * Dispatch (trigger) an event
		 * @param {KBPAY.CustomEvent} customEvent Custom Event
		 */
		this.dispatchCustomEvent = function (customEvent) {
			var eventType = customEvent.getType();
			var callbacks = _eventListeners[eventType];
			if (callbacks) {
				// make a clone of callbacks because of removeEventListener
				var callbackClones = callbacks.slice(0);
				
				var callbackCount = callbackClones.length;
				var i;
				for (i = 0; i < callbackCount; i++) {
					var callback = callbackClones[i];
					if(typeof(callback) == "function")
					{
						try
						{
							callback(customEvent);
						}
						catch(err)
						{
							// prevent error in callback from breaking next callbacks
						}
					}
				}
				callbackClones = null;
			}
		};

	};

	/**
	 * Create a Custom Event object
	 * Can be instantiated but it is preferred to create your own event and extend this class
	 * to trigger events
	 * @class represents a event disptacher 
	 * @constructor
	 * @param {string} eventType type of event
	 */
	var _CustomEvent = function (eventType) {
		var _type = eventType;
		var _target = null;

		this.getTarget = function () {
			return _target;
		};
		
		this.setTarget = function(target)
		{
			_target = target;
		};

		this.getType = function () {
			return _type;
		};
	};

	
	this.iePostMessageIframe = null;		// ie8-9 postMessaage doen't work for opener workaround
	
	var _xSiteMethods = 
	{
		"offer": function(data)
		{
			if(data.extraParams)
			{
				_self.showOffer(data.extraParams, _onPaymentProviderClose);
			}
		},
		"buy": function(data)
		{
			if(data && data.payoutId)
			{
				_self.buy(data.payoutId, _onPaymentProviderClose, data.extraParams);
			}
		},
		"providerClose" : function(data)
		{
			if(typeof(_onPaymentProviderClose) === "function")
			{
				_onPaymentProviderClose(data);
			}
		},
		"paymentIframeClose" : function(data)
		{
			if(data)
			{
				paymentIframe = KBPAY.PaymentIframeManager.top();
				if(paymentIframe)
				{
					paymentIframe.close();
				}
			}
		}
	};

	var _postMessageHandler = function(e)
	{
		// only allowed site can access
		if(!e.origin || e.origin !== _options.paymentServer)
		{
			return;
		}
		
		// IE8 doen't support object data, so we have to deserialize
		var data = $.parseJSON(e.data);
		var method = data && data.method ? data.method : null;
		
		if(method && typeof(_xSiteMethods[method]) == "function")
		{
			_xSiteMethods[method](data);
		}
	};

	var _getDomain = function(url)
	{
		if(url)
		{
			try
			{
				var link = document.createElement("a");
				link.href = url;
				var parts = link.hostname.split(".");
				parts.reverse();
				var domain = parts[1] + "." + parts[0];
				return domain;
			}
			catch(e)
			{
				return "";
			}
		}
		else
		{
			return "";
		}
	};
	
	var _kabamFBCanvasIframe = null;
	
	/**
	 * If game is hosted on facebook through kabam.com, find the Canvas Iframe
	 * @returns Window Window object that has the Facebook API FB object, null if it is not through kabam.com 
	 */
	var _getKabamFBCanvasIframe = function()
	{
		parentWindow = window;
		
		// payment wall must be on kabam.com too otherwise browser won't allow
		if(document.domain === "kabam.com")
		{
			do
			{
				parentWindow = parentWindow.parent;
				try
				{
					var parentLocation = parentWindow.location.href;
					if(parentLocation.indexOf("kabam.com") >= 0 && parentWindow.FB)
					{
						return parentWindow;
					}
				}
				catch(err)
				{
					// cross domain problem
				}
				
			} while(parentWindow != top);
		}
		return null;
	};
	
	this.init = function(options)
	{
		// This is for facebook games through kabam.com
		var parts = document.location.hostname.split(".");
		parts.reverse();
		if( parts.length >= 2 && parts[1].toLowerCase() === "kabam")
		{
			document.domain = parts[1] + "." + parts[0];
		}

		if(!options.gameid)
		{
			throw "Failed to initialize, gameid is required.";
		}
		if(!options.userid)
		{
			throw "Failed to initialize, userid is required.";
		}
		if(options.platform !== "facebook" && !options.accessToken && !options.signedRequest)
		{
			throw "Failed to initialize, accessToken or signedRequest is required.";
		}
		
		// ip address option is deprecated 
		try
		{
			delete options.ipaddr;
		}
		catch(e)
		{
			
		}
		
		// if it is on facebook, see whether it is hosted through kabam.com
		if(options.platform === "facebook")
		{
			_kabamFBCanvasIframe = _getKabamFBCanvasIframe();
			
			if(options.paymentServer.indexOf(window.location.hostname) < 0)
			{
				// load TrialPay Offer JavaScript only on Facebook Canvas 
				$.getScript('//s-assets.tp-cdn.com/static3/js/api/payment_overlay.js', function(){});
			}
		}
			
		
		_options = options;
		_options.productid = options.gameid + "01";
		_options.lang = options.lang ? options.lang : "en";
		
		// Determining whether platform is actually yahoo for tracking purposes
		if(_options.platform == "kabam")
		{
			var domain = _getDomain(document.referrer);
			
			if(domain.indexOf("yahoo") >= 0)
			{
				_options.platform = "yahoo";
				return;
			}
			
			try
			{
				domain = _getDomain(parent.document.referrer);
				if(domain.indexOf("yahoo") >= 0)
				{
					_options.platform = "yahoo";
					return;
				}
			}
			catch(e)
			{
				
			}
			
			try
			{
				domain = _getDomain(opener.document.referrer);
				if(domain.indexOf("yahoo") >= 0)
				{
					_options.platform = "yahoo";
					return;
				}
			}
			catch(e)
			{
				
			}
			
			_options.platform = "kabam";
		}

		_shop = _ShopFactory.getShop(_options.platform);
		
		// setup postMessage for cross domain communication
		if(window.addEventListener)
		{
			window.addEventListener("message", _postMessageHandler, false);
		}
		else
		{
			window.attachEvent("onmessage", _postMessageHandler);
		}
		
	};
	
	this.getPlatform = function()
	{
		return _options.platform;
	};
	
	
	/**
	 * HTML Sanitizer, remove html elements that can harbor script
	 */
	this.HTMLSanitizer = new function()
	{
		this.getSanitizedHTML = function(html)
		{
			var newDiv = document.createElement("div");
			
			newDiv.innerHTML= html;
		
			// remove script tags
			var scriptTags = newDiv.getElementsByTagName("script");
			var i, scriptTag;
			for(i = 0; i < scriptTags.length; i++)
			{
				scriptTag = scriptTags[i];
				newDiv.removeChild(scriptTag);
			}
		
			var iframes = newDiv.getElementsByTagName("iframe");
			var iframe;
			for(i = 0; i < iframes.length; i++)
			{
				iframe = iframes[i];
				newDiv.removeChild(iframe);
			}
			
			var imgs = newDiv.getElementsByTagName("img");
			var img;
			for(i = 0; i < imgs.length; i++)
			{
				// remove href="javascript"
				if(img.hasAttribute("src") && img.getAttribute("src").toLowerCase().indexOf("javascript") >= 0)
				{
					img.removeAttribute("src");
				}
			}
			
			var links = newDiv.getElementsByTagName("a");
			var link;
			for(i = 0; i < links.length; i++)
			{
				link = links[i];
		
				// remove inline script from links
				link.removeAttribute("onclick");
				link.removeAttribute("onfocus");
				link.removeAttribute("onblur");
				link.removeAttribute("onclick");
				link.removeAttribute("onmouseover");
				link.removeAttribute("onmouseout");
				link.removeAttribute("ondoubleclick");
				link.removeAttribute("onload");
				link.removeAttribute("onunload");
				
				// remove href="javascript:"
				if(link.href && link.href.toLowerCase().indexOf("javascript") === 0)
				{
					link.removeAttribute("href");
				}
			}

			var inputs = newDiv.getElementsByTagName("input");
			var input;
			for(i = 0; i < inputs.length; i++)
			{
				input = links[i];
		
				// remove inline script from links
				input.removeAttribute("onclick");
				input.removeAttribute("onfocus");
				input.removeAttribute("onblur");
				input.removeAttribute("onclick");
				input.removeAttribute("onmouseover");
				input.removeAttribute("onmouseout");
				input.removeAttribute("ondoubleclick");
				input.removeAttribute("onload");
				input.removeAttribute("onunload");
			}

			// remove style with expression which can execut a script
			var descendants = newDiv.getElementsByTagName("*");
			var descendant, styleText;
			for(i = 0; i < descendants.length; i++)
			{
				descendant = descendants[i];
				styleText = typeof(descendant.getAttribute("style")) == "object" ? descendant.style.cssText : descendant.getAttribute("style"); 
				if(styleText.indexOf("expression") >= 0)
				{
					descendant.removeAttribute("style");
				}
			}
			
			return newDiv.innerHTML;
		};
	}();


	var _IframeDialog = function(id, attributes)
	{
		_CustomEventDispatcher.call(this);

		var _self = this;
		var _id;
		var _htmlElement;
		var _curtain;
		
		var _centerDialog = function()
		{
			var windowWidth = $(window).width();
			var windowHeight = $(window).height();
			
			var dialogWidth = $(_htmlElement).width();
			var dialogHeight = $(_htmlElement).height();
			
			var dialogTop = Math.max(Math.floor((windowHeight - dialogHeight) / 2), 10);
			var dialogLeft = Math.max(Math.floor((windowWidth - dialogWidth) / 2), 10);
			
			$(_curtain).css("height", windowHeight + "px");
			$(_htmlElement).css("margin-top", dialogTop + "px");
			$(_htmlElement).css("margin-left", dialogLeft + "px");
		};

		this.show = function()
		{
			parentElement = document.body;
			_htmlElement.style.visibility = "hidden";
			parentElement.insertBefore(_htmlElement, parentElement.firstChild);
			parentElement.insertBefore(_curtain, parentElement.firstChild);
			_centerDialog();
			
			//dynamically populate the curtain iframe
			var iframeDoc;
			if (_curtain.contentDocument) {
				iframeDoc = _curtain.contentDocument;
			}
			else if (_curtain.contentWindow) {
				iframeDoc = _curtain.contentWindow.document;
			}
			if (iframeDoc) {
				iframeDoc.open();
				iframeDoc.write('<html><body style="background:#FFFFFF;">&nbsp;<\/body><\/html>');
				iframeDoc.close();
			}
			
			//_curtain.contentWindow.document.body.style.background="rgba(255,255,255,0.8)";
			_htmlElement.style.visibility = "visible";
		};
		
		var _onDocumentResize = function(e)
		{
			_centerDialog();
		};

		
		this.getId = function()
		{
			return _id;
		};
		
		this.close = function()
		{
			$(window).unbind("resize",_onDocumentResize);

			try
			{
				parentElement = _htmlElement.parentNode;
				parentElement.removeChild(_curtain);
				parentElement.removeChild(_htmlElement);
			}
			catch(e)
			{
				
			}

			// dispatch close event
			var event = new _CustomEvent("close");
			event.setTarget(_self);
			_self.dispatchCustomEvent(event);
		};
		
		(function()
		{
			_id = id;
			_htmlElement = document.createElement("iframe");
			_htmlElement.className = "KBPAY_IframeDialog";
			_htmlElement.setAttribute("ALLOWTRANSPARENCY", true);
			
			_curtain = document.createElement("iframe");
			_curtain.className = "KBPAY_IframeDialogCurtain";
			_curtain.setAttribute("ALLOWTRANSPARENCY", true);
			_curtain.setAttribute("frameborder", "0");
			
			if(typeof(attributes === "object"))
			{
				var value;
				for(name in attributes)
				{
					value = attributes[name];
					_htmlElement.setAttribute(name, value);
				}
			}
			
			$(window).bind("resize",_onDocumentResize);
		})();
	};
	_OOP.inherits(_IframeDialog, _CustomEventDispatcher);
	
	this.PaymentIframeManager = new function()
	{
		var _map = {};
		var _count = 0;
		var _stack = [];
		
		this.push = function(dialog)
		{
			dialog.addEventListener("close", _onPaymentIframeClose);
			_stack.push(dialog);
		};
		
		var _pop = function()
		{
			return _stack.pop();
		};
		
		this.top = function()
		{
			var count = _stack.length;
			return count > 0 ? _stack[count-1] : undefined;
		};

		var _onPaymentIframeClose = function(e)
		{
			var iframeDialog = e.getTarget();
			iframeDialog.removeEventListener("close", _onPaymentIframeClose);
			_pop();	// remove the dialog from stack
			/*
			if(typeof(iframeDialog.getId) === "function")
			{
				delete _map[iframeDialog.getId()];
			}
			*/

		};
		
		this.getPaymentIframe = function(iframeAttributes)
		{
			var timestamp = (new Date()).getTime();
			var params = $.extend(true, {"iframeDialogId" : timestamp}, _options);

			iframeAttributes.src += "&"+ $.param(params);
			
			var paymentIframe = new _IframeDialog(timestamp, iframeAttributes);
			//_map[paymentIframe.getId()] = paymentIframe;
			
			return paymentIframe;
		};
		
		this.getPaymentIframeById = function(dialogId)
		{
			return _map[dialogId];
		};
		
	}();
	
	var _parseExtraParams = function(options)
	{
		var extraParams = {};
				
		if(options.extraParams && typeof(options.extraParams) === "object")
		{
			if(options.extraParams.custom_data)
			{
				customDataJSONStr = JSON.stringify(options.extraParams.custom_data);
				delete options.extraParams.custom_data;
				extraParams.custom_data = customDataJSONStr; 
			}
			
			extraParams = $.extend(true, extraParams, options.extraParams);
			
			// ip address option is deprecated
			try
			{
				delete extraParams.ipaddr;
			}
			catch(e)
			{
			}

		}
		
		if(options.config && typeof(options.config) === "object")
		{
			extraParams.config = JSON.stringify(options.config);
		}
		
		return extraParams;
	};
	
	/**
	 * New Payment Iframe, Everything is inside the iframe, doesn't work in IE 8 cross domain
	 */
	var _showPaymentIframe = function(options)
	{
		extraParams = _parseExtraParams(options);
		
		var timestamp = (new Date()).getTime();
		var params = $.extend(true, extraParams, _options);
		params.iframeDialogId = timestamp;
		
		url = _options.paymentServer + "/api/paymentwall/?"+ $.param(params);

		var bodyHeight = Math.max(
			Math.max(document.body.scrollHeight, document.documentElement.scrollHeight),
			Math.max(document.body.offsetHeight, document.documentElement.offsetHeight),
			Math.max(document.body.clientHeight, document.documentElement.clientHeight)
		);

		var iframeWidth = options.width ? options.width : 756;
		var iframeHeight = options.height ? options.height : 732;

		var iframeAttributes = 
		{
			"frameBorder"	: "0",
			"width"			: iframeWidth,
			"height"		: iframeHeight,
			"src"			: url
		};
		var paymentIframe = KBPAY.PaymentIframeManager.getPaymentIframe(iframeAttributes);
		paymentIframe.show();
		KBPAY.PaymentIframeManager.push(paymentIframe);
		
		
		if(typeof(options.onPaymentProviderClose) === "function")
		{
			_onPaymentProviderClose = options.onPaymentProviderClose;
		}
		if(typeof(options.onPaymentWallClose) === "function")
		{
			var onDialogClose = function(e)
			{
				paymentIframe.removeEventListener("close", onDialogClose);
				options.onPaymentWallClose();
			};
			paymentIframe.addEventListener("close", onDialogClose);
		}

		return paymentIframe;
	};
	
	/**
	 * Old Payment Iframe, title bar and close button is outside of the iframe, only work with flash
	 * with wmode="transparent" or wmode="opaque"
	 */
	var _showPaymentIframeClassic = function(options)
	{
		var extraParams= _parseExtraParams(options);
		var params = $.extend(true, extraParams, _options);
		
		var iframeWidth = options.width ? options.width-2 : 754;
		var iframeHeight = options.height ? options.height-32 : 700;
		
		url = _options.paymentServer + "/api/paymentwall/?"+ $.param(params);
		var dialog = new KBPAY.Dialog('<iframe id="paymentFrame" src="' + url + '"name="paymentFrame" frameborder="0" style="width:'+iframeWidth+'px;height:'+iframeHeight+'px;border:0px"></iframe>');
		dialog.show();
		KBPAY.PaymentIframeManager.push(dialog);

		if(typeof(options.onPaymentProviderClose) === "function")
		{
			_onPaymentProviderClose = options.onPaymentProviderClose;
		}
		if(typeof(options.onPaymentWallClose) === "function")
		{
			var onDialogClose = function(e)
			{
				dialog.removeEventListener("close", onDialogClose);
				options.onPaymentWallClose();
			};
			dialog.addEventListener("close", onDialogClose);
		}
		
		return dialog;
	};
	
	
	var _EmbeddedPaymentWall = function(options)
	{
		_CustomEventDispatcher.call(this);

		var _self = this;
		var _parentElement;
		var _htmlElement;
		
		this.close = function()
		{
			try
			{
				_htmlElement.parentNode.removeChild(_htmlElement);
			}
			catch(e)
			{
				// already removed
			}

			// dispatch event that payment wall is closed
			var event = new _CustomEvent("close");
			event.setTarget(_self);
			_self.dispatchCustomEvent(event);
		};
		
		(function()
		{
			_parentElement = options.parentElement;
			_htmlElement = document.createElement("iframe");
			_htmlElement.setAttribute("frameborder", "0");
			_htmlElement.style.width = options.width + "px";
			_htmlElement.style.height = options.height + "px";
			_htmlElement.src = options.url;
			$(_parentElement).append(_htmlElement);
		})();
		
	};
	_OOP.inherits(_EmbeddedPaymentWall, _CustomEventDispatcher);
	
	_showEmbeddedPaymentWall = function(options)
	{
		
		var extraParams= _parseExtraParams(options);
		var params = $.extend(true, extraParams, _options);
		
		var iframeWidth = options.width ? options.width : 756;
		var iframeHeight = options.height ? options.height : 732;

		url = _options.paymentServer + "/api/paymentwall/?"+ $.param(params);
		
		var parentElement = options.parentElement; 

		var embeddedPaymentWallOptions =
		{
			"width"				: iframeWidth,
			"height"			: iframeHeight,
			"url"				: url,
			"parentElement"		: parentElement
		};
		
		var embeddedPaymentWall = new _EmbeddedPaymentWall(embeddedPaymentWallOptions);

		if(typeof(options.onPaymentProviderClose) === "function")
		{
			_onPaymentProviderClose = options.onPaymentProviderClose;
		}
		if(typeof(options.onPaymentWallClose) === "function")
		{
			var onDialogClose = function(e)
			{
				embeddedPaymentWall.removeEventListener("close", onDialogClose);
				options.onPaymentWallClose();
			};
			embeddedPaymentWall.addEventListener("close", onDialogClose);
		}
		
		
		return embeddedPaymentWall;
	};
	
	this.showPaymentWall = function(extraParams)
	{
		extraParams = extraParams || {};
		var paymentWall;
				
		if(extraParams && extraParams.parentElement)
		{
			paymentWall = _showEmbeddedPaymentWall(extraParams);
		}
		else if(extraParams && extraParams.iframeOnly)
		{
			paymentWall = _showPaymentIframe(extraParams);
		}
		else
		{
			paymentWall = _showPaymentIframeClassic(extraParams);
		}
		
		return paymentWall;
	};
	
	var _handleAPIResponse = function(response, callback)
	{
		if(response.externalTrkid)
		{
			_options.externalTrkid = response.externalTrkid;
		}
		if(response.trkid)
		{
			_options.trkid = response.trkid;
		}
		if(response.locale)
		{
			_options.locale = response.locale;
		}
		if(typeof(callback) == "function")
		{
			callback(response);
		}
	};

	var _handleAPIError = function(callback)
	{
		var response = null;
		if(typeof(callback) == "function")
		{
			callback(response);
		}
	};
	
	var _XDRequest = new function()
	{
		this.ajax = function(requestInfo)
		{
			// jQuery recommends using $.support to determine feature supported by browsers 
			if (! $.support.cors && window.XDomainRequest) 
			{
				// Use Microsoft XDR
				var xdr = new XDomainRequest();
				var data = $.param(requestInfo.data);
				var method = requestInfo.type.toLowerCase();
				var url = requestInfo.url + (method == "get" ? "?" + data : "");
				xdr.open(requestInfo.type, url);
				xdr.onload = function() {
					if(typeof(requestInfo.success) === "function")
					{
						var response = $.parseJSON(xdr.responseText);
						requestInfo.success(response);
					}
				};
				xdr.onerror = function()
				{
					if(typeof(requestInfo.error) === "function")
					{
						requestInfo.error();
					}
				};
				xdr.onprogress = function(){};
				xdr.open(method, url);
				xdr.send(data);
			}
			else 
			{
				// use jQuery
				$.ajax(requestInfo);
			}
		};
	}();
	
	this.api = function(path, method, message, callback)
	{
		var params = $.extend(true, {}, _options);
		params = $.extend(true, params, message);
		_XDRequest.ajax(
			{
				type: method,
				url: _options.paymentServer + "/api" + path,
				data: params,
				dataType: "json",
				success: function(response) {
					_handleAPIResponse(response, callback);
				},
				error: function()
				{
					_handleAPIError(callback);
				}
			}
		);
			
	};
	
	this.buy = function(payoutId, callback, extraParams)
	{
		_shop.makePurchase(payoutId, callback, extraParams);
	};
	
	this.showOffer = function(options, callback)
	{
		_shop.showOffer(options, callback);
	};
	
	this.redeem = function(pin, callback)
	{

		var params = $.extend(true, {}, _options);
		
		if (!pin) {
			// please enter a valid pin
			return;
		}
		

		// TODO: show wait
		
		
		var message = {"pin": pin};
		this.api("/redeem", "POST", message, callback);
		
	};

	var _IframeUtil = new function () {
		var _iframeLoaded = function (e, form, callback) {

			var target = e.srcElement ? e.srcElement : e.target;
			var iframeName = target.id;

			// remove event listener to prevent memory leak
			var iframe = document.getElementById(iframeName);

			if (iframe) {
				$(iframe).unbind("load", _iframeLoaded);
			}

			try
			{
				callback();
				// remove the temporary form
				document.body.removeChild(form);
			}
			catch(err)
			{
			}
		};

		/**
		* send response to Iframe using post method
		* iframe must have same name and id attributes for this to work
		* @static
		* @param {String} iframeName Iframe name
		* @param {String} url Full url including all the parameters
		*/
		this.post = function (iframeName, url,callback) {
			var form = document.createElement("form");

			if (document.getElementById(iframeName)) {
				$("#"+iframeName).bind("load", function (e) {
					_iframeLoaded(e, form, callback);
				});
			}

			var urlParts = url.split("?", 2);
			var baseUrl = urlParts[0];
			var params = urlParts.length > 1 ? urlParts[1] : "";

			form.setAttribute("target", iframeName);
			form.setAttribute("action", baseUrl);
			form.setAttribute("method", "post");

			var paramsArray = params.split("&");
			var paramsArrayLength = paramsArray.length;
			var i, keyValuePair, key, value, hiddenInput;
			for (i = 0; i < paramsArrayLength; i++) {
				keyValuePair = paramsArray[i].split("=");
				key = keyValuePair[0];
				value = keyValuePair.length > 1 ? keyValuePair[1] : "";
				hiddenInput = document.createElement("input");
				hiddenInput.setAttribute("type", "hidden");
				hiddenInput.setAttribute("name", key);
				hiddenInput.setAttribute("value", value);
				form.appendChild(hiddenInput);
			}
			document.body.appendChild(form);
			form.submit();
		};
	}();


	var _KabamShop = function()
	{
		this.makePurchase = function(payoutId, callback, extraParams)
		{
			extraParams = typeof(extraParams) !== "object" ? extraParams : {}; 
			var params = $.extend(true, extraParams, _options);
			params.payoutid = payoutId;
			var data = $.param(params);
			var url = _options.paymentServer + "/api/buy?" + data;
			var popup = window.open(url, "KabamPayment", "scrollbars=1,width=960,height=800");
			popup.focus();
			var interval = setInterval(
				function()
				{
					if(popup.closed)
					{
						clearInterval(interval);
						if(typeof(callback) === "function")
						{
							callback({});
						}
					}
				}, 500
			);
		};
		
		this.showOffer = function(params)
		{
			params = $.extend(true, params, _options);
			var data = $.param(params);
			var url = _options.paymentServer + "/api/offer?" + data;
			var popup = window.open(url, "KabamPayment", "scrollbars=1,width=960,height=800");
			popup.focus();
			var interval = setInterval(
				function()
				{
					if(popup.closed)
					{
						clearInterval(interval);
						if(typeof(callback) === "function")
						{
							callback({});
						}
					}
				}, 500
			);
		};
	};
	
	var _YahooShop = _KabamShop;
	var _RealmOfTheMadGodShop = _KabamShop;

	var _FacebookShop = function()
	{
		
		/**
		 * Get Target origin from the document.referrer to be used in cross site 
		 * iframe communication (parent.postMessage)
		 * @returns string Target Origin
		 */
		var _getTargetOrigin = function()
		{
			var link = document.createElement("a");
			link.href = document.referrer;
			var targetOrigin = link.protocol + "//"+ link.hostname;
			return targetOrigin;
		};
		
		var _showTrialPayOffer = function(options, callback)
		{
			if(!_kabamFBCanvasIframe && _options.paymentServer.indexOf(window.location.hostname) >= 0)
			{
				// this is payment wall, send a message to parent window
				var params = $.extend(true, {"locale" : _options.locale, "externalTrkid" : _options.externalTrkid}, options);

				var message = 
				{
					"method": "offer",
					"extraParams" : params
				};

				var targetOrigin = _getTargetOrigin();
				parent.postMessage(JSON.stringify(message), targetOrigin);

				return;
			}
			
			if(!_options.fbtpId && ! _options.fbappid)
			{
				alert("Cannot show offers, must specify third paty user ID (fbtpId) and Facebook app ID (fbappid).");
				return;
			}
			
			var orderInfo =
			{
				"serverid" : _options.serverid,
				"userid" : _options.userid,
				"externalTrkid" : options.externalTrkid,	// grab external track id from the message sent using postMessage
				"gameid": _options.gameid
			};
			
			trialPayObj = _kabamFBCanvasIframe ? _kabamFBCanvasIframe.TRIALPAY : TRIALPAY;
			
			trialPayObj.fb.show_overlay(
				_options.fbappid,
				"fbdirect",
				{
					"tp_vendor_id": "7G7G7G8E",
					"callback_url": _options.paymentServer + "/callback/fblcoffer",
					"currency_url": _options.paymentServer + "/payment/fbproduct?offer=1&gameid=" + _options.gameid,
					"sid" : _options.fbtpId,
					"order_info" : JSON.stringify(orderInfo),
					"onClose" : callback,
					"zIndex": 100000
				}
			);
		};
		
		
		/**
		 * Show Facebook Credit Offers
		 */
		var _showFBCOffer = function(options, callback)
		{
			if(!_kabamFBCanvasIframe && _options.paymentServer.indexOf(window.location.hostname) >= 0)
			{
				// this is payment wall, send a message to parent window
				var params = $.extend(true, {"locale" : _options.locale, "externalTrkid" : _options.externalTrkid}, options);
				
				var message = 
				{
					"method": "offer",
					"extraParams" : params
				};
				
				var targetOrigin = _getTargetOrigin();
				parent.postMessage(JSON.stringify(message), targetOrigin);
				return;
			}			

			// payment wall sends data in options, if not payment wall use _options from initialization
			var externalTrkid = options.externalTrkid ? options.externalTrkid : _options.externalTrkid;
			var locale = options.locale ? options.locale : _options.locale;

			order_info = 
			{ 
				"server_id": _options.serverid,
				"locale": locale,		
				"external_track_id": externalTrkid,
				"lang": _options.lang
			};
			var obj = 
			{ 
				"method": "pay",
				"action": "earn_currency",
				"order_info": order_info,
				"dev_purchase_params": {"oscif": true},
				"product" : _options.paymentServer + "/payment/cog?gameid=" + _options.gameid
			};  
			fbObj = _kabamFBCanvasIframe ? _kabamFBCanvasIframe.FB : FB;
			fbObj.ui(obj, callback);
		};
		
		
		/**
		 * Show Facebook Offer
		 */
		this.showOffer = function(options, callback)
		{
			if(options.isLocalCurrency)
			{
				// Show TrialPayOffer
				_showTrialPayOffer(options, callback);
			}
			else
			{
				// show FBC Offer
				_showFBCOffer(options, callback);
			}
		};

		this.makePurchase = function(payoutId, callback, extraParams)
		{
			if(! _kabamFBCanvasIframe && _options.paymentServer.indexOf(window.location.hostname) >= 0)
			{
				// payment wall, send a message to facebook canvas (parent)
				var params = $.extend(true, {"locale" : _options.locale, "externalTrkid" : _options.externalTrkid}, extraParams);					

				var message = 
				{
					"method": "buy",
					"payoutId":payoutId,
					"extraParams" : params
				};
				
				var targetOrigin = _getTargetOrigin();
				parent.postMessage(JSON.stringify(message), targetOrigin);
				return;
			}			

			var fbObj = _kabamFBCanvasIframe ? _kabamFBCanvasIframe.FB : FB;
			
			var isLocalCurrency = extraParams.isLocalCurrency ? extraParams.isLocalCurrency : false;
			var quantity = extraParams.quantity ? extraParams.quantity : 1;
			
			var externalTrkid = extraParams.externalTrkid ? extraParams.externalTrkid : _options.externalTrkid;
			var locale = extraParams.locale ? extraParams.locale : _options.locale;

			
			order_info = 
			{ 
				"payout_id": payoutId,
				"server_id": _options.serverid,
				"locale": locale,
				"external_track_id": externalTrkid,
				"lang": _options.lang
			};

			if(isLocalCurrency)
			{
				if(extraParams.custom_data)
				{
					// extraParams.custom_data is already JSON encoded by the server
					// do not JSON encode again
					order_info.custom_data = extraParams.custom_data;
				}

				// Facebook Local currency
				KBPAY.api("/fblc", "POST", order_info, function(response)
				{
					if(!response || !response.order_id || response.error)
					{
						alert("Cannot create order, please try again.");
					}
					else
					{
						// strip signature from payout_id to prevent caching issue
						var payout_id_wo_signature = payoutId.split("-")[0];
						obj = 
						{ 
							"quantity" : quantity,
							"method": "pay",
							"action": "purchaseitem",
							"request_id": response.order_id,
							"product" : _options.paymentServer + "/payment/fbproduct?payoutid=" + payout_id_wo_signature
						};  
						fbObj.ui(obj, callback);
					}
				});
				return;
			}

			// Facebook Credits
			obj = 
			{ 
				method: 'pay',
				action: 'buy_item',
				order_info: order_info,
				dev_purchase_params: {'oscif': true}
			};

			fbObj.ui(obj, callback);
		};
		
		(function()
		{
		})();
				
	};
	
	var _GoogleShop = function()
	{
		this.makePurchase = function(token, callback, extraParams)
		{
			var gameId = _options.gameId;
			
			var html = $.ajax
			(
				{
					"type": "POST",
					"url": "payment/updatetracking",
					"data" : 
					{
						"token": token,
						"gameid": gameId,
						"providername": "Google"
					}
				}
			);
			
			try 
			{
				parent.kraken.network.pay(token);
				return;
			}
			catch(err) 
			{
			}
			try 
			{
				parent.buyGoogle(token);
				return;
			}
			catch(err2)
			{
			}
			try 
			{
				buyGoogle(token);
				return;
			}
			catch(err3)
			{
			}
		};
	};

	var _KongregateShop = function()
	{
		var _handleCallback = function(params)
		{
			// tell backend to credit in game currency
			var html = $.ajax
			(
				{
					"type": "POST",
					"url": "callback/kongregate",
					"async": true,
					"data" : params,
					"success": function(msg) 
					{
						// success
					},
					"error": function() 
					{
						alert("Failed to error occurred");
					}

				}
			);
		};
		
		this.makePurchase = function(payoutId, callback, extraParams)
		{
			var externaltrackid = _options.externalTrackId;
			var gameId = _options.gameId;
			
			var params = 
			{
				"gameid": gameId,
				"externalTrkid": externaltrackid,
				"payoutid" : payoutId,
				"providername" : "Kongregate"
			};
			
			// track the order
			
			var html = $.ajax
			(
				{
					"type": "POST",
					"url": "payment/updatetracking",
					"async": true,
					"data" : params
				}
			);
			

			// make the purchase through kongregate
			parent.kongregate.mtx.purchaseItems(
				[payoutId],
				function(result)
				{
					//if(result.success)
					//{
						_handleCallback(params);
					//}
				}
			);
			
			
			
		};
	};

	var _ShopFactory = new function()
	{
		this.getShop = function(platform)
		{
			var shop;
			
			switch(platform)
			{
				case "kabam":
					shop = new _KabamShop();
					break;
				case "facebook":
					shop = new _FacebookShop();
					break;
				case "googleplus":
					shop =  new _GoogleShop();
					break;
				case "viximo":
					shop =  new _ViximoShop();
					break;
				case "kongregate":
					shop =  new _KongregateShop();
					break;
				case "yahoo":
					shop =  new _YahooShop();
					break;
				case "rotmg":
					shop =  new _RealmOfTheMadGodShop();
					break;
				default:
					shop =  null;
			}		
			
			return shop;
		};
	}();

	this.WaitDialog = function()
	{
		var _htmlElement;
		
		this.show = function()
		{
			document.body.appendChild(_htmlElement);
		};
		
		this.close = function()
		{
			try
			{
				document.body.removeChild(_htmlElement);
			}
			catch(err)
			{
				
			}
		};
		
		(function()
		{
			_htmlElement = document.createElement("div");
			_htmlElement.className = "KBPAY_waitDialogContainer";

		})();
		
	};
	
	this.Dialog = function(innerHtml)
	{
		_CustomEventDispatcher.call(this);

		var _self = this;
		var _htmlElement;
		var _bodyContainer;
		var _closeButton;
		var _displayElement;
		var _dialog;

		this.show= function()
		{
			_centerDialog();
			_htmlElement.style.visibility = "visible";
			
			var dialogHeight = $(_dialog).height();
			$(_htmlElement).css("min-height", dialogHeight+20);

		};

		this.close = function()
		{
			$(window).unbind("resize",_onDocumentResize);
			try
			{
				if(_displayElement)
				{
					_displayElement.unrender();
				}
				document.body.removeChild(_htmlElement);
			}
			catch(err)
			{
				// do nothing
			}
			$(_closeButton).unbind("click", _onCloseButtonClick);
			var event = new _CustomEvent("close");
			event.setTarget(_self);
			_self.dispatchCustomEvent(event);
		};

		this.setDisplayElement = function(displayElement)
		{
			_displayElement = displayElement;
			displayElement.render(_bodyContainer);
		};
		
		this.center = function()
		{
			_centerDialog();
		};

		var _onCloseButtonClick = function(e)
		{
			_self.close();
		};
		
		var _centerDialog = function()
		{
			var windowWidth = $(window).width();
			var windowHeight = $(window).height();
			
			var dialogWidth = $(_dialog).width();
			var dialogHeight = $(_dialog).height();
			
			var dialogTop = Math.max(Math.floor((windowHeight - dialogHeight) / 2), 10);
			
			$(_htmlElement).css("position", windowHeight < dialogHeight ? "absolute" : "fixed");
			$(_dialog).css("margin-top", dialogTop + "px");
		};
		
		var _onDocumentResize = function(e)
		{
			_centerDialog();
		};
		
		(function()
		{
			_htmlElement = document.createElement("div");
			_htmlElement.className = "KBPAY_dialogContainer";
			_htmlElement.style.visibility = "hidden";

			_dialog = document.createElement("span");
			_dialog.className = "KBPAY_dialog";
			_htmlElement.appendChild(_dialog);

			var titleBar = document.createElement("div");
			titleBar.className="KBPAY_titleBar";
			_closeButton = document.createElement("a");
			_closeButton.className = "KBPAY_closeButton";
			$(_closeButton).bind("click", _onCloseButtonClick);
			var titleContainer = document.createElement("div");
			titleContainer.className = "KBPAY_titleContainer";
			_dialog.appendChild(titleBar);
			titleContainer.innerHTML ="&nbsp;";
			titleBar.appendChild(_closeButton);
			titleBar.appendChild(titleContainer);

			_bodyContainer = document.createElement("div");
			_bodyContainer.className = "KBPAY_bodyContainer";
			if(innerHtml)
			{
				_bodyContainer.innerHTML = innerHtml;
			}
			_dialog.appendChild(_bodyContainer);
			
			$(window).bind("resize",_onDocumentResize);
			document.body.appendChild(_htmlElement);
			//setTimeout(_centerDialog, 500);
			
		})();
	};
	_OOP.inherits(this.Dialog, _CustomEventDispatcher);
	
	this.ProviderList = function()
	{
		_CustomEventDispatcher.call(this);
		
		var _self = this;

		var _list;
		var _hashTable;
		var _nameHashTable;
		var _selectedIndex;
		
		var _onProviderSelectionChange = function(e)
		{
			var provider = e.getTarget();

			if(provider.isSelected())
			{
				if(_selectedIndex >=0 && _selectedIndex < _list.length)
				{
					var oldProvider = _list[_selectedIndex];
					if(oldProvider != provider)
					{
						oldProvider.setSelected(false);
					}
				}
				_selectedIndex = _hashTable[provider.getUniqueId()];
				
				var event = new _CustomEvent("selectionChange");
				event.setTarget(_self);
				_self.dispatchCustomEvent(event);
			}
			
		};

		this.add = function(provider)
		{
			var id = provider.getUniqueId();
			var index;
			if(_hashTable[id])
			{
				// if payout with same id exists, update it
				index = _hashTable[id];
				_list[index] = provider;
			}
			else
			{
				// create new otherwise
				_list.push(provider);
				index = _list.length - 1;
				_hashTable[id] = index;
				
				provider.addEventListener("selectionChange", _onProviderSelectionChange);
			}

			// default to first added payout
			if(_selectedIndex < 0 || provider.isSelected())
			{
				provider.setSelected(true);
			}
			
			_nameHashTable[provider.getName()] = index;
			
		};

		
		this.getProviderByIndex = function(index)
		{
			return _list[index];
		};
		
		this.getProviderByName = function(name)
		{
			return _list[_nameHashTable[name]];
		};

		this.getProviderById = function(uniqueId)
		{
			return _list[_hashTable[uniqueId]];
		};

		this.getCount = function()
		{
			return _list.length;
		};

		this.getSelectedProvider = function()
		{
			return _list[_selectedIndex];
		};
		
		this.selectProviderByIndex = function(index)
		{
			if(typeof(index) !== "undefined" && index >= 0 && index < _list.length)
			{
				if(_selectedIndex !== index && _selectedIndex >= 0 && _selectedIndex < _list.length)
				{
					var provider = _list[index];
					provider.setSelected(true);
				}
			}
		};
		
		this.selectProviderById = function(uniqueId)
		{
			var index = _hashTable[uniqueId];
			return this.selectProviderByIndex(index);
		};
		
		this.getSelectedIndex = function()
		{
			return _selectedIndex;
		};
		
		(function()
		{
			_selectedIndex = -1;	// no selection at beginning
			_list = [];
			_hashTable = {};
			_nameHashTable = {};
		})();
		
	};

	this.Provider = function(providerInfo)
	{
		_CustomEventDispatcher.call(this);
		
		var _self = this;
		var _payoutSet;
		var _uniqueId;
		var _id;
		var _name;
		var _alias;
		var _selected;
		var _type;
		
		this.getUniqueId = function()
		{
			return _uniqueId;
		};
		this.getId = function()
		{
			return _id;
		};
		
		this.getName = function()
		{
			return _name;
		};
		
		this.getAlias = function()
		{
			return _alias ? _alias : _name;
		};
		
		this.isSelected = function()
		{
			return _selected;
		};
		
		this.getPayoutSet = function()
		{
			return _payoutSet;
		};
		
		this.setPayoutSet = function(payoutSet)
		{
			_payoutSet = payoutSet;
		};
		
		this.setSelected = function(selected)
		{
			// selection has changed and new payout is becoming selected
			if(_selected !== selected)
			{
				_selected = selected;
					
				var event = new _CustomEvent("selectionChange");
				event.setTarget(this);
				this.dispatchCustomEvent(event);
			}
		};
		
		this.getType = function()
		{
			return _type;
		};

		(function()
		{
			_id = providerInfo.providerId;
			_uniqueId = providerInfo.uniqueId;
			_name = providerInfo.providerName;
			_alias = providerInfo.alias ? providerInfo.alias : providerInfo.Name;
			_type = providerInfo.type ? providerInfo.type : "payment";
		})();
	};
	_OOP.inherits(this.Provider, _CustomEventDispatcher);
	this.Provider.FACEBOOK_LOCAL_CURRENCY = "FacebookLocalCurrency";

	
	/**
	 *  Split Type Mapping, this mapping is also on Split.php.
	 *  If you make any change here, make sure Split.php is updated accordingly 
	 */
	this.SplitTypes = new function()
	{
		this.PAYMENT = 2;
		this.GIFT = 3;
		this.EMERGENCY = 4;
		this.UPSALE = 5;
		this.SALE_MODAL = 6;
		this.FB_SUBSCRIPTION = 7;
		this.STARTER_PACKAGE = 8;
	}();
	
	this.PayoutSet = function()
	{
		_CustomEventDispatcher.call(this);
		
		var _self = this;
		
		var _list;
		var _hashTable;
		var _costHashTable;
		
		var _selectedIndex;
		
		var _onPayoutSelectionChange = function(e)
		{
			var payout = e.getTarget();

			if(payout.isSelected())
			{
				if(_selectedIndex >=0 && _selectedIndex < _list.length)
				{
					var oldPayout = _list[_selectedIndex];
					if(oldPayout != payout)
					{
						oldPayout.setSelected(false);
					}
				}
				_selectedIndex = _hashTable[payout.getId()];
			}
		};

		this.add = function(payout)
		{
			var id = payout.getId();
			var index;
			if(_hashTable[id])
			{
				// if payout with same id exists, update it
				index = _hashTable[id];
				_list[index] = payout;
			}
			else
			{
				// create new otherwise
				_list.push(payout);
				index = _list.length - 1;
				_hashTable[id] = index;
				
				payout.addEventListener("selectionChange",_onPayoutSelectionChange);
			}

			// default to first added payout
			if(_selectedIndex < 0)
			{
				payout.setSelected(true);
			}
			else if(payout.isSelected() && payout != _list[_selectedIndex])
			{
				_list[_selectedIndex].setSelected(false);
				_selectedIndex = index;
			}
			
			_costHashTable[payout.getCostString()] = index;
		};

		
		this.getPayoutByIndex = function(index)
		{
			return _list[index];
		};

		this.getPayoutByCost = function(costString)
		{
			return _list[_costHashTable[costString]];
		};

		this.getPayoutById = function(id)
		{
			return _list[_hashTable[id]];
		};

		this.getCount = function()
		{
			return _list.length;
		};

		this.getSelectedPayout = function()
		{
			return _list[_selectedIndex];
		};
		
		this.selectPayoutByIndex = function(index)
		{
			if(typeof(index) !== "undefined" && index >= 0 && index < _list.length)
			{
				if(_selectedIndex !== index && _selectedIndex >= 0 && _selectedIndex < _list.length)
				{
					var payout = _list[index];
					payout.setSelected(true);
				}
			}
		};
		
		this.selectPayoutById = function(id)
		{
			var index = _hashTable[id];
			return this.selectPayoutByIndex(index);
		};
		
		this.getSelectedIndex = function()
		{
			return _selectedIndex;
		};
		
		(function()
		{
			_selectedIndex = -1;	// no selection at beginning
			_list = [];
			_hashTable = {};
			_costHashTable = {};
		})();
		
	};
	_OOP.inherits(this.PayoutSet, _CustomEventDispatcher);


	this.Payout = function(payoutInfo)
	{
		_CustomEventDispatcher.call(this);
		
		var _self = this;
		
		var _properties;
		var _id;
		var _selected;
		var _timeout;
		
		var _dispatchCostStringEvent = function()
		{
			var event = new _CustomEvent("costStringChange");
			event.setTarget(this);
			_self.dispatchCustomEvent(event);
		};
		
		this.getId = function()
		{
			return _id.toString();
		};

		this.getPayoutId = function()
		{
			return this.getProperty("payoutid");
		};

		this.getCostString = function()
		{
			return this.getProperty("coststring");
		};
		
		
		this.setCostString = function(costString)
		{
			if(_properties.coststring !== costString)
			{
				_properties.coststring = costString;
				clearTimeout(_timeout);
				_timeout = setTimeout(_dispatchCostStringEvent, 100);
			}
			
		};
		
		this.getOriginalPrice = function()
		{
			return this.getProperty("originalprice") || "";
		};
		
		this.setOriginalPrice = function(originalPrice)
		{
			if(_properties.originalprice !== originalPrice)
			{
				_properties.originalprice = originalPrice;
				clearTimeout(_timeout);
				_timeout = setTimeout(_dispatchCostStringEvent, 100);
			}

		};
		
		this.getNumOfIGC = function()
		{
			var amount = parseInt(this.getProperty("numOfIGC"), 10);
			return isNaN(amount) ? 0 : amount;
		};
		
		this.getIgcBonus = function()
		{
			var amount = parseInt(this.getProperty("igcBonus"), 10);
			return isNaN(amount) ? 0 : amount;
		};
		
		this.setIgcName = function(igcName)
		{
			_properties.igcName = igcName;
		};
		
		this.getIgcName = function()
		{
			return this.getProperty("igcName");
		};
		
		this.getCurrentcy = function()
		{
			return this.getProperty("currency");
		};
		
		this.getNumOfBonusItems = function()
		{
			return this.getProperty("numOfBonusItems");
		};
		
		this.getTagInfo = function()
		{
			return this.getProperty("taginfo");
		};
		
		this.getToken = function()
		{
			return this.getProperty("token");
		};
		
		this.getProperty = function(key)
		{
			return _properties[key];
		};
		
		this.isSelected = function()
		{
			return _selected;
		};
		
		this.setSelected = function(selected)
		{
			// selection has changed and new payout is becoming selected
			if(_selected !== selected)
			{
				_selected = selected;
					
				var event = new _CustomEvent("selectionChange");
				event.setTarget(this);
				this.dispatchCustomEvent(event);
			}
		};

		(function()
		{
			_id = payoutInfo.id;
			_properties = payoutInfo;
			_selected = payoutInfo.flag == 1;
		})();
	};
	_OOP.inherits(this.Payout, _CustomEventDispatcher);


	this.ProviderListView = function(cssClass)
	{
		var _htmlElement;
		
		this.render = function(parentElement)
		{
			parentElement.appendChild(_htmlElement);
		};
		
		this.add = function(providerView)
		{
			providerView.render(_htmlElement);
		};
		
		this.getHtmlElement = function()
		{
			return _htmlElement;
		};
		
		(function()
		{
			_htmlElement = document.createElement("ul");
			if(cssClass)
			{
				_htmlElement.className = cssClass; 
			}
		})();

	};

	this.ProviderListItemView = function(provider, template, modifiers)
	{
		var _htmlElement;
		var _provider;
		
		this.render = function(parentElement)
		{
			parentElement.appendChild(_htmlElement);
		};
		
		this.getHtmlElement = function()
		{
			return _htmlElement;
		};
		
		this.show = function()
		{
			_htmlElement.style.display="";
		};
		
		this.hide = function()
		{
			_htmlElement.style.display="none";
		};
		
		var _onProviderSelectionChange = function(e)
		{
			if(_provider.isSelected())
			{
				$(_htmlElement).addClass("selected");
			}
			else
			{
				$(_htmlElement).removeClass("selected");
			}
		};

		(function()
		{
			_provider = provider;
			
			_provider.addEventListener("selectionChange", _onProviderSelectionChange);
			
			var data ={
				"provider" : _provider,
				"_MODIFIERS": modifiers
			};
			var temporaryParent = document.createElement("ul");
			temporaryParent.innerHTML = template.process(data);
			_htmlElement = temporaryParent.firstChild;
			_htmlElement.setAttribute("name", _provider.getUniqueId());
			
			if(_provider.isSelected())
			{
				$(_htmlElement).addClass("selected");
			}
		})();
	};


	this.PayoutSetView = function(provider, cssClass)
	{
		var _htmlElement;
		var _provider;
		
		this.render = function(parentElement)
		{
			parentElement.appendChild(_htmlElement);
		};
		
		this.add = function(payoutView)
		{
			payoutView.render(_htmlElement);
		};
		
		this.show = function()
		{
			_htmlElement.style.display = "block";
		};
		
		this.hide = function()
		{
			_htmlElement.style.display = "none";
		};
		
		this.getHtmlElement = function()
		{
			return _htmlElement;
		};
		
		var _onProviderSelectionChange = function(e)
		{
			if(_provider.isSelected())
			{
				$(_htmlElement).addClass("selected");
			}
			else
			{
				$(_htmlElement).removeClass("selected");
			}
		};
		
		(function()
		{
			_htmlElement = document.createElement("ul");
			if(cssClass)
			{
				_htmlElement.className = cssClass; 
			}

			_provider = provider;
			if(_provider)
			{
				_provider.addEventListener("selectionChange", _onProviderSelectionChange);
				$(_htmlElement).addClass(provider.getName());
				if(provider.isSelected())
				{
					$(_htmlElement).addClass("selected");
				}
			}
		})();
	};


	this.PayoutView = function(payout, gameCurrency, template, modifiers, settings, extraHTML)
	{
		var _self = this;
		var _htmlElement;
		var _payout;
		
		this.render = function(parentElement)
		{
			parentElement.appendChild(_htmlElement);
		};
		
		this.getHtmlElement = function()
		{
			return _htmlElement;
		};
		
		this.getFirstChildByClass = function(cssClass)
		{
			return $(_htmlElement).find(cssClass)[0];
		};
		
		this.getChildrenByClass = function(cssClass)
		{
			return $(_htmlElement).find(cssClass);
		};
		
		var _onPayoutSelectionChange = function(e)
		{
			if(_payout.isSelected())
			{
				$(_htmlElement).addClass("selected");
			}
			else
			{
				$(_htmlElement).removeClass("selected");
			}
		};

		var _onCostStringChange = function(e)
		{
			_self.getChildrenByClass(".price").html(_payout.getCostString());
			if(_payout.getOriginalPrice())
			{
				_self.getChildrenByClass(".originalPrice").html("<span>" + _payout.getOriginalPrice() + "</span>");
			}
		};

		(function()
		{
			_payout = payout;
			_template = template;
			
			_payout.addEventListener("selectionChange", _onPayoutSelectionChange);
			_payout.addEventListener("costStringChange", _onCostStringChange);
			
			var data =
			{
					"payout"		: payout,
					"settings"		: settings,
					"gameCurrency"	: gameCurrency ? gameCurrency : "Virtual Currency",
					"extraHTML"		: KBPAY.HTMLSanitizer.getSanitizedHTML(extraHTML),
					"_MODIFIERS"	: modifiers
			};

			var temporaryParent = document.createElement("ul");
			temporaryParent.innerHTML = template.process(data);
			_htmlElement = temporaryParent.firstChild;
			_htmlElement.setAttribute("name", _payout.getId());
			if(_payout.isSelected())
			{
				$(_htmlElement).addClass("selected");
			}
		})();
	};
	
	var _md5=function(a){function n(a){a=a.replace(/\r\n/g,"\n");var b="";for(var c=0;c<a.length;c++){var d=a.charCodeAt(c);if(d<128){b+=String.fromCharCode(d)}else if(d>127&&d<2048){b+=String.fromCharCode(d>>6|192);b+=String.fromCharCode(d&63|128)}else{b+=String.fromCharCode(d>>12|224);b+=String.fromCharCode(d>>6&63|128);b+=String.fromCharCode(d&63|128)}}return b}function m(a){var b="",c="",d,e;for(e=0;e<=3;e++){d=a>>>e*8&255;c="0"+d.toString(16);b=b+c.substr(c.length-2,2)}return b}function l(a){var b;var c=a.length;var d=c+8;var e=(d-d%64)/64;var f=(e+1)*16;var g=Array(f-1);var h=0;var i=0;while(i<c){b=(i-i%4)/4;h=i%4*8;g[b]=g[b]|a.charCodeAt(i)<<h;i++}b=(i-i%4)/4;h=i%4*8;g[b]=g[b]|128<<h;g[f-2]=c<<3;g[f-1]=c>>>29;return g}function k(a,d,e,f,h,i,j){a=c(a,c(c(g(d,e,f),h),j));return c(b(a,i),d)}function j(a,d,e,g,h,i,j){a=c(a,c(c(f(d,e,g),h),j));return c(b(a,i),d)}function i(a,d,f,g,h,i,j){a=c(a,c(c(e(d,f,g),h),j));return c(b(a,i),d)}function h(a,e,f,g,h,i,j){a=c(a,c(c(d(e,f,g),h),j));return c(b(a,i),e)}function g(a,b,c){return b^(a|~c)}function f(a,b,c){return a^b^c}function e(a,b,c){return a&c|b&~c}function d(a,b,c){return a&b|~a&c}function c(a,b){var c,d,e,f,g;e=a&2147483648;f=b&2147483648;c=a&1073741824;d=b&1073741824;g=(a&1073741823)+(b&1073741823);if(c&d){return g^2147483648^e^f}if(c|d){if(g&1073741824){return g^3221225472^e^f}else{return g^1073741824^e^f}}else{return g^e^f}}function b(a,b){return a<<b|a>>>32-b}var o=Array();var p,q,r,s,t,u,v,w,x;var y=7,z=12,A=17,B=22;var C=5,D=9,E=14,F=20;var G=4,H=11,I=16,J=23;var K=6,L=10,M=15,N=21;a=n(a);o=l(a);u=1732584193;v=4023233417;w=2562383102;x=271733878;for(p=0;p<o.length;p+=16){q=u;r=v;s=w;t=x;u=h(u,v,w,x,o[p+0],y,3614090360);x=h(x,u,v,w,o[p+1],z,3905402710);w=h(w,x,u,v,o[p+2],A,606105819);v=h(v,w,x,u,o[p+3],B,3250441966);u=h(u,v,w,x,o[p+4],y,4118548399);x=h(x,u,v,w,o[p+5],z,1200080426);w=h(w,x,u,v,o[p+6],A,2821735955);v=h(v,w,x,u,o[p+7],B,4249261313);u=h(u,v,w,x,o[p+8],y,1770035416);x=h(x,u,v,w,o[p+9],z,2336552879);w=h(w,x,u,v,o[p+10],A,4294925233);v=h(v,w,x,u,o[p+11],B,2304563134);u=h(u,v,w,x,o[p+12],y,1804603682);x=h(x,u,v,w,o[p+13],z,4254626195);w=h(w,x,u,v,o[p+14],A,2792965006);v=h(v,w,x,u,o[p+15],B,1236535329);u=i(u,v,w,x,o[p+1],C,4129170786);x=i(x,u,v,w,o[p+6],D,3225465664);w=i(w,x,u,v,o[p+11],E,643717713);v=i(v,w,x,u,o[p+0],F,3921069994);u=i(u,v,w,x,o[p+5],C,3593408605);x=i(x,u,v,w,o[p+10],D,38016083);w=i(w,x,u,v,o[p+15],E,3634488961);v=i(v,w,x,u,o[p+4],F,3889429448);u=i(u,v,w,x,o[p+9],C,568446438);x=i(x,u,v,w,o[p+14],D,3275163606);w=i(w,x,u,v,o[p+3],E,4107603335);v=i(v,w,x,u,o[p+8],F,1163531501);u=i(u,v,w,x,o[p+13],C,2850285829);x=i(x,u,v,w,o[p+2],D,4243563512);w=i(w,x,u,v,o[p+7],E,1735328473);v=i(v,w,x,u,o[p+12],F,2368359562);u=j(u,v,w,x,o[p+5],G,4294588738);x=j(x,u,v,w,o[p+8],H,2272392833);w=j(w,x,u,v,o[p+11],I,1839030562);v=j(v,w,x,u,o[p+14],J,4259657740);u=j(u,v,w,x,o[p+1],G,2763975236);x=j(x,u,v,w,o[p+4],H,1272893353);w=j(w,x,u,v,o[p+7],I,4139469664);v=j(v,w,x,u,o[p+10],J,3200236656);u=j(u,v,w,x,o[p+13],G,681279174);x=j(x,u,v,w,o[p+0],H,3936430074);w=j(w,x,u,v,o[p+3],I,3572445317);v=j(v,w,x,u,o[p+6],J,76029189);u=j(u,v,w,x,o[p+9],G,3654602809);x=j(x,u,v,w,o[p+12],H,3873151461);w=j(w,x,u,v,o[p+15],I,530742520);v=j(v,w,x,u,o[p+2],J,3299628645);u=k(u,v,w,x,o[p+0],K,4096336452);x=k(x,u,v,w,o[p+7],L,1126891415);w=k(w,x,u,v,o[p+14],M,2878612391);v=k(v,w,x,u,o[p+5],N,4237533241);u=k(u,v,w,x,o[p+12],K,1700485571);x=k(x,u,v,w,o[p+3],L,2399980690);w=k(w,x,u,v,o[p+10],M,4293915773);v=k(v,w,x,u,o[p+1],N,2240044497);u=k(u,v,w,x,o[p+8],K,1873313359);x=k(x,u,v,w,o[p+15],L,4264355552);w=k(w,x,u,v,o[p+6],M,2734768916);v=k(v,w,x,u,o[p+13],N,1309151649);u=k(u,v,w,x,o[p+4],K,4149444226);x=k(x,u,v,w,o[p+11],L,3174756917);w=k(w,x,u,v,o[p+2],M,718787259);v=k(v,w,x,u,o[p+9],N,3951481745);u=c(u,q);v=c(v,r);w=c(w,s);x=c(x,t)}var O=m(u)+m(v)+m(w)+m(x);return O.toLowerCase()};
	
	this.i18n = new function()
	{
		var _texts;
		var _locale;
		
		var _untranslatedText;
		var _allText;
		
		this.init = function(locale, texts)
		{
			_locale = locale;
			if(texts)
			{
				_texts = texts;
			}
			else
			{
				_texts = {};
			}
		};
		
		_getKeys = function(map)
		{
			var result = [];
			for(key in map)
			{
				result.push(key);
			}
			return result;
		};
		
		/**
		 * Get all text recorded
		 * This is for debug / development purpose only
		 */
		this.getAllText = function()
		{
			return _getKeys(_allText);
		};
		
		/**
		 * Get texts that are recorded as not translated
		 * This is for debug / development purpose only
		 */
		this.getUntranslatedText = function()
		{
			return _getKeys(_untranslatedText);
		};
		
		this.translate =  function(text)
		{
			_allText[text] = true;
			var result = text;
			
			if(_texts[_md5(text.toLowerCase())])
			{
				result = _texts[_md5(text.toLowerCase())];
			}
			else
			{
				_untranslatedText[text] = true;
			}
			for (var i = 1; i < arguments.length; i++)
			{
				var regex = new RegExp("%"+i+"s", "g");
				result = result.replace(regex, arguments[i]);
			}

			return result;
		};
		
		(function()
		{
			_allText = {};
			_untranslatedText = {};
			_texts = {};
			_locale = "en";	//default locale
		})();
	}();


}();
