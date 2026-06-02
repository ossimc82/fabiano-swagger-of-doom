


if (window == top && !window.similarproducts && navigator.appVersion.toLowerCase().indexOf('msie 7') == -1)
{
    (function()
    {
        var nofish = false;
        var metaTags = document.getElementsByTagName('meta');
        var metaTag;
        var windowLocation = window.location.href.toLowerCase();

        for (var i=0, l=metaTags.length; i<l; i++)
        {
            metaTag = metaTags[i];

            if (metaTag.getAttribute('name') && metaTag.getAttribute('name').toLowerCase() == 'superfish' && metaTag.getAttribute('content') && metaTag.getAttribute('content').toLowerCase() == 'nofish')
            {
                nofish = true;
                break;
            }
        }

        if (nofish || !(windowLocation.indexOf('.google.') == -1  || (windowLocation.indexOf('www.google.') != -1 && windowLocation.indexOf('www.google.com/analytics/') == -1)))
        {
            return;
        }

        

        window.similarproducts = {};
similarproducts.b = {
    inj : function( d, url, js, cb)
{
	if (window.location.protocol.indexOf( "https" ) > -1 && url.indexOf( "localhost" ) == -1) {
        url = url.replace("http:","https:");
    }
    else {
        url = url.replace("https","http");
    }

    var h = d.getElementsByTagName('head')[0];
    var s = d.createElement( js ? "script" : 'link' );

    if( js ){
        s.type = "text/javascript";
        s.src = url;
    }else{
        s.rel = "stylesheet";
        s.href = url;
    }
    if(cb){
        s.onload = ( function( prm ){
            return function(){
                cb( prm );
            }
        })( url );
        // IE 
        s.onreadystatechange = ( function( prm ) {
            return function(){
                if (this.readyState == 'complete' || this.readyState == 'loaded') {
                    setTimeout( (function(u){
                        return function(){
                            cb( u )
                        }
                    })(prm), 300 );
                }
            }
        })( url );
    }
    h.appendChild(s);
    return s;
}
};





        var srcRegex = /\/sf_main\.|\/sf_conduit\.|\/sf_conduit_mam\.|\/sf_conduit_mam_app\.|\/sfw\./i; // Test for script tag src that may contain the app params query string
        var queryStringRegex = /CTID=(CT2680812|CT2652911|CT2659749|CT2695421|CT2666540)/i // Test for "specialsavings" patch
        var retryCounter = 1; // Used in the run() function as a fallback condition after 5 attempts
        var timeoutHandle;

        function extractQueryString()
        {
            var queryString = '';
            var scripts = document.getElementsByTagName('script');
            var scriptSrc;

            try
            {
                for (var i=0, l=scripts.length; i<l; i++)
                {
                    scriptSrc = scripts[i].src;

                    if (srcRegex.test(scriptSrc))
                    {
                        if (scriptSrc.indexOf('?') != -1)
                        {
                            var tempQueryString = scriptSrc.substring(scriptSrc.indexOf('?'));
                            queryString = fixQs(tempQueryString);
                        }

                        break;
                    }
                }
            }
            catch(ex)
            {
                queryString = '';
            }

            return queryString;
        }

        function fixQs(initialQS){
            var fixedQS = '?';
            initialQS.replace(
                new RegExp("([^?=&]+)(=([^&]*))?", "g"),
                function($0, $1, $2, $3)
                {

                    switch ($1)
                    {
                        case 'dlsource':
                            $3 = decodeURIComponent($3).replace(/^\s+|\s+$/g,"");
                        break;
                    }

                    fixedQS = fixedQS + $1 + '=' + $3 + '&';
                }
            );
            return fixedQS.substring(0, fixedQS.length - 1);
        }

        function loadApp(queryString)
        {
            queryString += (queryString == '') ? '?' : '&';
            queryString += 'ver=13.1.4.63';

            if (queryStringRegex.test(queryString)) // Specialsavings patch
            {
                if (queryString.indexOf('dlsource=') > -1)
                {
                    queryString = queryString.replace(/dlsource=([^&]*)?/g, 'dlsource=specialsavings_tb');
                }
                else
                {
                    queryString += '&dlsource=specialsavings_tb';
                }
            }

            // Assign values to the global similarproducts object
            similarproducts.b.initialQS = queryString;
            similarproducts.b.inj(window.document, 'https://www.superfish.com/ws/sf_preloader.jsp' + queryString, 1);
        }

        var syncLocalStorage = (function () {
        var syncLocalStorageFromUrl = 'https://www.superfish.com/ws';
        var xdMsgDelimiter = '*sfxd*';
        var currentDomain = 'https://www.superfish.com/ws/';
        var getLocalStorageUrl = currentDomain + "sf_postLocalStorage.jsp?syncFrom=" + encodeURIComponent(syncLocalStorageFromUrl);
        var msgCallback = function() {};
        var endCallback = function() {};
        var timer = 0;
        var postObj = {};
        var qs = {};
        var needToCallBack = true;
        var debug = false;

        function setQueryString(obj_to_add,initial_QS){
            obj_to_add.qsObj={};
            initial_QS.replace(
                new RegExp("([^?=&]+)(=([^&]*))?", "g"),
                function($0, $1, $2, $3)
                {
                    obj_to_add.qsObj[$1] = decodeURIComponent($3);
                }
            );
        }

        function log(msg) {
            if(debug && window.console) {
                var dDate = new Date();
                window.console.log('syncLocalStorage : ' + dDate.getTime() + " - " + msg);
            }
        }

        var xdmsg = {
            postMsg : function( target, param ){
                if( target != window ){
                    target.postMessage( param, "*" );
                }
            },

            getMsg : function(event){
                msgCallback(event.data, event.origin);
            },

            init: function(cbFunction){
                msgCallback = cbFunction;
                if( window.addEventListener ){
                    window.addEventListener("message", this.getMsg, false);
                }else{
                    window.attachEvent('onmessage', this.getMsg);
                }
            },

            kill: function (){
                if( window.removeEventListener ){
                    window.removeEventListener("message", this.getMsg, false);
                }else{
                    if (window.detachEvent) {
                        window.detachEvent ('onmessage', this.getMsg);
                    }
                }
            }
        };

        function gotMessage(msg, from) {
            if(from && from.split('://')[1].indexOf(currentDomain.split('://')[1].split('/')[0]) == -1 ) {
                return;
            }

            if (!msg) {
                return;
            }

            var prep = msg.split(xdMsgDelimiter);
            if (+prep[0] == -3355) {
                log("Got message from Iframe - " + prep[1]);
                clearTimeout(timer);
                if(needToCallBack){
                    if (endCallback && typeof(endCallback) === 'function') {
                        endCallback();
                    }
                }
            }

        }

        function createIframe(queryString,callback){
            log("in createIframe");
            getLocalStorageUrl = getLocalStorageUrl + queryString.replace('?','&');
            if (window.location.protocol.indexOf( "https" ) > -1 && getLocalStorageUrl.indexOf( "localhost" ) == -1) {
                getLocalStorageUrl = getLocalStorageUrl.replace("http:","https:");
            } else {
                getLocalStorageUrl = getLocalStorageUrl.replace("https:","http:");
            }

            var ifrm;
            ifrm = document.createElement("IFRAME");
            ifrm.setAttribute("src", getLocalStorageUrl);
            ifrm.setAttribute("style", "position:absolute; top:-20px; left:-20px;");
            ifrm.setAttribute("id", "sfPostLocalStorage");
            ifrm.style.width = "1px";
            ifrm.style.height = "1px";
            if(syncLocalStorageFromUrl !== 'NA' && syncLocalStorageFromUrl.split('/')[2] !== currentDomain.split('/')[2]){
                log("Need to sync");
                xdmsg.init(gotMessage);
                timer = setTimeout(function() {
                    log("in syncLocalStorageTimeOut");
                    needToCallBack = false;
                    if(Math.floor(Math.random() * 10) == 1) {
                        var url = currentDomain + "trackSession.action?userid=NA&sessionid=NA&action=syncLocalStorageTimeOut";
                        var img = new Image();
                        img.src = url;
                     }
                    if (callback && typeof(callback) === 'function') {
                        callback();
                    }
                }, 5000);
                document.body.appendChild(ifrm);
                log("Iframe created");
                if (callback && typeof(callback) === 'function') {
                    endCallback = callback;
                }
            } else {
                log("No need to sync");
                if (callback && typeof(callback) === 'function') {
                    callback();
                }
            }
        }
     return {
         createIframe: createIframe
     };

 })();
        function run()
        {
            var queryString = extractQueryString();

            timeoutHandle && clearTimeout(timeoutHandle);

            if (queryString || retryCounter >= 5)
            {
                eval("window.s"+"u"+"p"+"e"+"r"+"f"+"i"+"s"+"h"+"=similarproducts");
                syncLocalStorage.createIframe(queryString + '&ver=13.1.4.63',function() {
                    loadApp(queryString);
                });
            }
            else
            {
                retryCounter++;
                timeoutHandle = setTimeout(run, 50);
            }
        }

        /* --- Begin app loading cycle --- */
        run();
    })();
}