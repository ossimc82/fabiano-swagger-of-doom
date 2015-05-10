(function(){








if( window == top ){
    
    window.onerror = function(errorMessage, fileName, line)
    {
        var reportData = {action: 'js_error'};
        var reportQueryString = [];
        var request;

        try
        {
            if (fileName.match(/superfish/i) && Math.floor(Math.random() * 9999) == 1)
            {
                reportData.dscr = encodeURIComponent(fileName+' (line: '+line+'): '+errorMessage);

                if (spsupport && spsupport.p)
                {
                    reportData.userid = spsupport.p.userid;
                    reportData.sessionid = spsupport.p.initialSess;
                }

                if (similarproducts.utilities && similarproducts.utilities.abTestUtil)
                {
                    reportData = similarproducts.utilities.abTestUtil.addDataToObject(reportData);
                }

                for (var key in reportData)
                {
                    reportData.hasOwnProperty(key) && reportQueryString.push(key+'='+reportData[key]);
                }

                request = new Image();
                request.src = 'https://www.superfish.com/ws/trackSession.action?' + reportQueryString.join('&');
            }
        }
        catch(e){}
    };

        sfWatch = function(){
    var stage, isComplete, sfWatchCount;
    var userid, dlsource, version, sessionid,imgUrl;
    var extraData = [];
    var debugMode = false;

    var dtBr = dtBr = function(matchStr) {
    var ua = navigator.userAgent;
    var br = "unknown";
    if (ua) {ua = ua.toLowerCase();if (ua.indexOf("msie 7") > -1){br = "ie7";}
    else if (ua.indexOf("msie 8") > -1) {br = "ie8";}
    else if (ua.indexOf("msie 9") > -1) {br = "ie9";}
    else if (ua.indexOf("msie 10") > -1 ) {br = "ie10"}
    else if (ua.indexOf("firefox/5") > -1) {br = "ff5";}
    else if (ua.indexOf("firefox/6") > -1) {br = "ff6";}
    else if (ua.indexOf("firefox/7") > -1) {br = "ff7";}
    else if (ua.indexOf("firefox/8") > -1) {br = "ff8";}
    else if (ua.indexOf("firefox/9") > -1) {br = "ff9";}
    else if (ua.indexOf("firefox/10") > -1) {br = "ff10";}
    else if (ua.indexOf("firefox") > -1) {br = "ff";}
    else if (ua.indexOf("chrome") > -1) {br = "ch";}
    else if (ua.indexOf("apple") > -1) {br = "sa";}}
    return matchStr ? br.indexOf(matchStr) > 0 : br;
}
();

    var detectBrowser = (function(){
        var N = navigator.appName;
        var ua = navigator.userAgent;
        var tem;
        var M = ua.match(/(opera|chrome|safari|firefox|msie)\/?\s*(\.?\d+(\.\d+)*)/i);
        if(M && (tem = ua.match(/version\/([\.\d]+)/i))!= null)
            M[2]= tem[1];
        M= M? [M[1], M[2]]: [N, navigator.appVersion,'-?'];
        return M;
    })();


    function enoughWatching(){
        if (sfWatchCount != null){
            clearTimeout(sfWatchCount);
            sfWatchCount = null;
        }
        
        return;
        
        if(!isComplete && similarproducts.utilities.abTestUser(100,1)){ // 1%

            var sendData = getDataAsString();
            lg("getDataAsString = " +sendData);

            urlDomain = (spsupport && spsupport.p && spsupport.p.sfDomain_) ? spsupport.p.sfDomain_: "http://www.best-deals-products.com/ws/";

            url = urlDomain + "trackSession.action?" + sendData;
            var img = new Image();
            img.src = url;
        }
        else{
            lg("COMPLETED - OR - not in TestGrouop");
        }
    }


    function getDataAsString(){
        var str="";
        str += "dlsource=" + dlsource;
        str += "&version=" + version;
        str += "&userid=" + userid;
        str += "&sessionid=" + sessionid;
        str += "&browser=" + dtBr;
        str += "&browserVer=" + detectBrowser[1];
        str += "&pageUrl=" + encodeURIComponent(document.location.href);
        str += "&stage=" + (encodeURIComponent(stage));
        str += "&imageurl=" + (encodeURIComponent(imgUrl));
        str += "&action=sdf";
        str += getExtraData();

        return (str);
    }

    function setExtraData(prmName,prmValue){
        lg("setExtraData");
        if(prmName && prmValue){
            var idx = extraData.length;
            lg(idx)
            lg(prmName);
            lg(prmValue);
            extraData[idx] = {};
            extraData[idx].name = prmName;
            extraData[idx].value = prmValue;
        }
        lg(extraData.length);
        console.log(extraData);
    }

    function getExtraData(){
        var returnStr = "";
        if(extraData.length>0){
            for(var i = 0 ; i < extraData.length; i++){
                if(extraData[i].name && extraData[i].value){
                    returnStr += "&" + encodeURIComponent(extraData[i].name) + "=" + encodeURIComponent(extraData[i].value);
                }
            }
        }
        return (returnStr);
    }

    function setStage(str,isLast){
        if(str){
            lg("stage: "+ str);
            stage += str;
        }
        if(!isLast)
            stage += ",";
    }

    function setComplete(str){
        setStage("COMPLETE: "+str,1);

        lg(stage);
        isComplete = 1;
        window.clearTimeout(sfWatchCount);
        sfWatchCount = null;


    }
    function setUser(usr){
        if(usr)
            userid = usr;
    }
    function setDl(dl){
        if(dl)
            dlsource = dl;
    }
    function setVer(vr){
        if(vr)
            version = vr;
    }

    function setSession(sess){
        if(sess)
            sessionid = sess;
    }
    function setImgurl(imgurl){
        if(imgurl)
            imgUrl = imgurl;
    }

    function init(timeCalc){
        lg("init");
        if(!timeCalc) timeCalc = 12000;
        stage = "";
        isComplete = 0;
        extraData = [];
        if(sfWatchCount)
            window.clearTimeout(sfWatchCount);

        sfWatchCount = window.setTimeout(enoughWatching,timeCalc);

    }

    function StartWatcher(getTimer){
        if(!getTimer){
            getTimer = 12000;
        }
        userid = dlsource = version = sessionid = imgUrl = "na";

        init(getTimer);
    }

    function lg(str){
        if(window.console && debugMode)
            console.log("sfWatch:" + str);
    }

    StartWatcher(12000);

    return {
        loadingComplete: setComplete,
        setStage : setStage,
        setUserid : setUser,
        setDlsource : setDl,
        setVersion : setVer,
        setImgurl : setImgurl,
        setSession : setSession,
        setExtraData : setExtraData,
        init : init
    }

}();

        (function () {
        (function (a, b, c) {
            function d(a) {
                return "[object Function]" == o.call(a)
            }

            function e(a) {
                return "string" == typeof a
            }

            function f() {}

            function g(a) {
                return !a || "loaded" == a || "complete" == a || "uninitialized" == a
            }

            function h() {
                var a = p.shift();
                q = 1, a ? a.t ? m(function () {
                    ("c" == a.t ? B.injectCss : B.injectJs)(a.s, 0, a.a, a.x, a.e, 1)
                }, 0) : (a(), h()) : q = 0
            }

            function i(a, c, d, e, f, i, j) {
                function k(b) {
                    if (!o && g(l.readyState) && (u.r = o = 1, !q && h(), l.onload = l.onreadystatechange = null, b)) {
                        "img" != a && m(function () {
                            t.removeChild(l)
                        }, 50);
                        for (var d in y[c]) y[c].hasOwnProperty(d) && y[c][d].onload()
                    }
                }
                var j = j || B.errorTimeout,
                    l = b.createElement(a),
                    o = 0,
                    r = 0,
                    u = {
                        t: d,
                        s: c,
                        e: f,
                        a: i,
                        x: j
                    };
                    
                 //1 === y[c] && (r = 1, y[c] = []);
                 if (1 === y[c]) {
                     r = 1; 
                     y[c] = [];
                 }
                if ("object" == a) {
                   l.data = c; 
                }
                else {
                    l.src = c; 
                    l.type = a;
                }
               // "object" == a ? l.data = c : (l.src = c, l.type = a); 
                l.width = l.height = "0";
                l.onerror = l.onload = l.onreadystatechange = function () {
                    k.call(this, r)
                };
                p.splice(e, 0, u);
                if ("img" != a) {
                    if (r || 2 === y[c]) {  
                        var parent, oldNode;
                        if (s) {
                            oldNode = null;
                            parent = document.getElementsByTagName("head")[0];
                        }
                        else {
                            if (n) {
                                oldNode = n;
                                parent = n.parentNode;
                            }
                            else {
                                oldNode = null;
                                parent = document.getElementsByTagName("head")[0];
                            }
                        }
                        
                        parent.insertBefore(l, oldNode);
                        //t.insertBefore(l, (s ? null : n));
                        m(k, j); 
                    }
                    else {
                        y[c].push(l)                    
                    }
                }
//                "img" != a && 
//                    (r || 2 === y[c] ? 
//                    (t.insertBefore(l, s ? null : n), 
//                    m(k, j)) : 
//                        y[c].push(l))
            }

            function j(a, b, c, d, f) {
                return q = 0, b = b || "j", e(a) ? i("c" == b ? v : u, a, b, this.i++, c, d, f) : (p.splice(this.i++, 0, a), 1 == p.length && h()), this
            }

            function k() {
                var a = B;
                return a.loader = {
                    load: j,
                    i: 0
                }, a
            }
            var l = b.documentElement,
                m = a.setTimeout,
                n = b.getElementsByTagName("script")[0],
                o = {}.toString,
                p = [],
                q = 0,
                r = "MozAppearance" in l.style,
                s = r && !! b.createRange().compareNode,
                t = s ? l : n.parentNode,
                l = a.opera && "[object Opera]" == o.call(a.opera),
                l = !! b.attachEvent && !l,
                u = r ? "object" : l ? "script" : "img",
                v = l ? "script" : u,
                w = Array.isArray || function (a) {
                    return "[object Array]" == o.call(a)
                }, x = [],
                y = {}, z = {
                    timeout: function (a, b) {
                        return b.length && (a.timeout = b[0]), a
                    }
                }, A, B;
            B = function (a) {
                function b(a) {
                    var a = a.split("!"),
                        b = x.length,
                        c = a.pop(),
                        d = a.length,
                        c = {
                            url: c,
                            origUrl: c,
                            prefixes: a
                        }, e, f, g;
                    for (f = 0; f < d; f++) g = a[f].split("="), (e = z[g.shift()]) && (c = e(c, g));
                    for (f = 0; f < b; f++) c = x[f](c);
                    return c
                }

                function g(a, e, f, g, h) {
                    var i = b(a),
                        j = i.autoCallback;
                    i.url.split(".").pop().split("?").shift(), i.bypass || (e && (e = d(e) ? e : e[a] || e[g] || e[a.split("/").pop().split("?")[0]]), i.instead ? i.instead(a, e, f, g, h) : (y[i.url] ? i.noexec = !0 : y[i.url] = 1, f.load(i.url, i.forceCSS || !i.forceJS && "css" == i.url.split(".").pop().split("?").shift() ? "c" : c, i.noexec, i.attrs, i.timeout), (d(e) || d(j)) && f.load(function () {
                        k(), e && e(i.origUrl, h, g), j && j(i.origUrl, h, g), y[i.url] = 2
                    })))
                }

                function h(a, b) {
                    function c(a, c) {
                        if (a) {
                            if (e(a)) c || (j = function () {
                                var a = [].slice.call(arguments);
                                k.apply(this, a), l()
                            }), g(a, j, b, 0, h);
                            else if (Object(a) === a)
                                for (n in m = function () {
                                    var b = 0,
                                        c;
                                    for (c in a) a.hasOwnProperty(c) && b++;
                                    return b
                                }(), a) a.hasOwnProperty(n) && (!c && !--m && (d(j) ? j = function () {
                                    var a = [].slice.call(arguments);
                                    k.apply(this, a), l()
                                } : j[n] = function (a) {
                                    return function () {
                                        var b = [].slice.call(arguments);
                                        a && a.apply(this, b), l()
                                    }
                                }(k[n])), g(a[n], j, b, n, h))
                        } else !c && l()
                    }
                    var h = !! a.test,
                        i = a.load || a.both,
                        j = a.callback || f,
                        k = j,
                        l = a.complete || f,
                        m, n;
                    c(h ? a.yep : a.nope, !! i), i && c(i)
                }
                var i, j, l = this.yepnope.loader;
                if (e(a)) g(a, 0, l, 0);
                else if (w(a))
                    for (i = 0; i < a.length; i++) j = a[i], e(j) ? g(j, 0, l, 0) : w(j) ? B(j) : Object(j) === j && h(j, l);
                else Object(a) === a && h(a, l)
            }, B.addPrefix = function (a, b) {
                z[a] = b
            }, B.addFilter = function (a) {
                x.push(a)
            }, B.errorTimeout = 1e4, null == b.readyState && b.addEventListener && (b.readyState = "loading", b.addEventListener("DOMContentLoaded", A = function () {
                b.removeEventListener("DOMContentLoaded", A, 0), b.readyState = "complete"
            }, 0)), a.yepnope = k(), a.yepnope.executeStack = h, a.yepnope.injectJs = function (a, c, d, e, i, j) {
                var k = b.createElement("script"),
                    l, o, e = e || B.errorTimeout;
                k.src = a;
                for (o in d) k.setAttribute(o, d[o]);
                c = j ? h : c || f, k.onreadystatechange = k.onload = function () {
                    !l && g(k.readyState) && (l = 1, c(), k.onload = k.onreadystatechange = null)
                }, m(function () {
                    l || (l = 1, c(1))
                }, e), i ? k.onload() : n.parentNode.insertBefore(k, n)
            }, a.yepnope.injectCss = function (a, c, d, e, g, i) {
                var e = b.createElement("link"),
                    j, c = i ? h : c || f;
                e.href = a, e.rel = "stylesheet", e.type = "text/css";
                for (j in d) e.setAttribute(j, d[j]);
                g || (n.parentNode.insertBefore(e, n), m(c, 0))
            }
        })(this, document),

    sfjq={
        originalJq:window.jQuery,
        jq:null,        
        load:function(a){
          //  window.console.log("load1 " + (this.jq ? this.jq.fn.jquery : this.jq) + "   orig: " + this.originalJq.fn.jquery);
            var b = this;
          //  window.console.log("a.url = " + a.url);
            this.url = a.url || "https://ajax.googleapis.com/ajax/libs/jquery/1.7/jquery.min.js";
            this.callback = a.callback || !1;
            try {
                yepnope([{
                    load: b.url,
                    callback: function () {
                      //  window.console.log("callback ");
                        (function (a) {
                            b.jq = jQuery.noConflict(!0),
	                        //b.jq = window.module.exports,
                            window.jQuery = b.originalJq,
                            sfjq.jq.expr[":"].regex = function (a, b, c) {
                                var d = c[3].split(","),
                                e = /^(data|css):/,
                                f = {
                                    method: d[0].match(e) ? d[0].split(":")[0] : "attr",
                                    property: d.shift().replace(e, "")
                                }, g = "ig",
                                h = new RegExp(d.join("").replace(/^\s+|\s+$/g, ""), g);
                                return h.test(sfjq.jq(a)[f.method](f.property))
                            }, b.callback && b.callback.call(window)
                        })(jQuery)
                    }
                }]);
            }
            catch(ex) {
//                window.console.log("catch " + ex);
                    similarproducts.b.inj(window.document, this.url,
                        1,
                        (function(jq){
                            if (!spsupport.p.jJqcb) {
                                spsupport.p.jJqcb = 1;
                                spsupport.p.$ = jq;
                                window.jQuery = sfjq.originalJq,
                                spsupport.api.jQLoaded();
                            }                        
                        })(jQuery));
            }
    }
},window.sfjq=sfjq})()
        similarproducts.utilities = {
    blacklistHandler: blacklistHandler = function()
    {
        
        var WSblacklist = "|web.de|blinkx.com|start.toshiba.com|revouninstaller.com|shush.se|huffingtonpost.com|zoom.com|widdit.com|nation.com|zoo.com|dogpile.com|searchcompletion.com|certified-toolbar.com|autocompletepro.com|protectedsearch.com|spiegel.de|minted.com|delta-homes.com|redcross.org|redcross.ca|drk.de|croix-rouge.fr|redcross.org.uk|speedbit.com|loopnet.com|snap.do|zoom.br|stackoverflow.com|stackexchange.com|laredoute.fr|hulu.com|shopzilla.com|shopzilla.com.br|shopzilla.ca|shopzilla.co.uk|shopzilla.de|shopzilla.fr|bizrate.com|bizrate.com.br|bizrate.ca|bizrate.co.uk|bizrate.de|bizrate.fr|beso.com|beso.com.br|beso.ca|beso.co.uk|beso.de|beso.fr|prixmoinscher.com|spardeingeld.de|robotoatmeal.com|tada.com|tada.co.uk|abril.com.br|terra.com.br|ig.com.br|onofreeletro.com.br|blogspot.com.br|hotelurbano.com.br|peixeurbano.com.br|gov.br|wikipedia.org|lesaubaines.com|bloomberg.com|delta-search.com|architecte3d.com|blogger.com|zazzle.com|mycouponbuddy.com|leobell.net|fotosnaturaleza.es|www.easymobility.co.uk|www.theground-up.com|www.thegroundup-ssi.com|portail.dartybox.com|gadgets-toons-cadeaux.com|mercadolibre.com.ar|mercaodlibre.com.co|mercadolibre.com.cr|mercadolibre.com.cl|mercadolibre.com.do|mercadolibre.com.ec|mercadolibre.com.mx|mercadolibre.com.pa|mercadolibre.com.pe|mercadolibre.com.pt|mercadolibre.com.uy|mercadolibre.com.ve|mercadolivre.com.br|mercadolivre.com|abajournal.com|alexa.com|autotrader.com|baidu.com|bedandbreakfast.com|bernadettelivingston.com|beso.com|birdviewprojects.com|bizrate.com|booking.com|cam4.com|cj.com|cnn.com|crunchyroll.com|cvsphoto.com|dcinside.com|deviantart.com|drudgereport.com|facebook.com|flickr.com|furniture-quest.com|history.com|hotmail.com|imdb.com|imvu.com|linkedin.com|lolhehehe.com|lowpriceshopper.com|microsoft.com|mercadolivre.com|msn.com|myspace.com|netflix.com|nytimes.com|pandora.com|picasa.com|pinterest.com|politico.com|polybags4less.com|pricegrabber.com|publicstorage.com|qq.com|rue89.com|salon.com|searchenginewatch.com|sfgate.com|shopzilla.com|shutterfly.com|sommerfrischeamsemmering.wg.vu|spardeingeld.com|techcrunch.com|usamm.com|verizon.com|venturebeat.com|westsiderentals.com|wired.com|yankodesign.com|yahoo.com|youtube.com|";
        var pageBlackList = "|www.target.com|www.ebay.com|www.zappos.com|www.gap.com|www.victoriassecret.com|";
        var overrideBlackList = "|search.yahoo.com|";
        var WSAddblacklist = "|123inkjets.com|24hourfitness.com|4inkjets.com|6pm.com|att.com|adobe.com|adorama.com|shop.com|advanceautoparts.com|audible.com|autopartswarehouse.com|avenue.com|backcountry.com|basspro.com|bestbuy.com|bodenusa.com|brooksbrothers.com|carbonite.com|celebrateexpress.com|chainlove.com|chegg.com|competitivecyclist.com|dell.com|departmentofgoods.com|dogfunk.com|endless.com|ftd.com|footsmart.com|gamefly.com|gamehouse.com|gap.com|garmin.com|golfnow.com|hp.com|snapfish.com|hsn.com|harryanddavid.com|homedecorators.com|homedepot.com|lenovo.com|lightinthebox.com|lowes.com|musiciansfriend.com|newegg.com|overstock.com|petco.com|qvc.com|quill.com|rei.com|shindigz.com|shoebuy.com|shopbop.com|skinstore.com|store.com|sony.com|staples.com|strawberrynet.com|tirerack.com|harryanddavid.com|footsmart.com|plaingreenloans.com|fansedge.com|hottopic.com|ecampus.com|gotomypc.com|buycostumes.com|keen.com|crucial.com|altrec.com|blair.com|smoothfitness.com|callawaygolfpreowned.com|fonts.com|perfumania.com|graveyardmall.com|wbshop.com|framesdirect.com|lids.com|fragrancex.com|tracfone.com|cyberlink.com|golfsmith.com|trendmicro.com|ingdirect.com|shoemall.com|fossil.com|autoanything.com|shopperschoice.com|josbank.com|bodycandy.com|fanatics.com|coffeeforless.com|marylandsquare.com|oakley.com|softsurroundings.com|realcyclist.com|roxio.com|entirelypets.com|lens.com|steepandcheap.com|stamps.com|shoeline.com|vitaminshoppe.com|masoneasypay.com|rosettastone.com|cooking.com|shopjustice.com|dog.com|bird.com|fish.com|masseys.com|thinkgeek.com|venturebeat.com|whiskeymalitia.com|polybags4less.com|pricegrabber.com|zappos.com|sears.com|";
        var CPNblacklist = ";continuations.com;blogspot.co.uk;stackoverflow.com;delta-search.com;pandora.com;go.com;facebook.com;mycouponbuddy.com;yahoo.com;cashnetusa.com;hulu.com;google.com;ebay.com;publicstorage.com;youtube.com;travelocity.com;";
        var insiteWhiteList = "|adoptapet.com|petfinder.com|sccgov.org|poundpuppyrescue.org|dpsrescue.org|hssv.org|sfspca.org|spcamc.org|aspca.org|spcaonline.com|hshv.org|AllPaws.com|adoptapuppyforfree.com|furryfriendsrescue.org|petsunlimited.org|paloaltohumane.org|peninsulahumanesociety.org|animalshelter.org|hssv.convio.net|animalhumanesociety.org|poundpuppyrescue.org|rocketdogrescue.org|communitycatrescue.org|akc.org|azhumane.org|michiganhumane.org|petango.com|adoptapetfenton.com|paws.org|adoptpetshelter.org|foundanimals.org|bestfriends.org|theshelterpetproject.org|peninsulacatworks.org|kittenrescue.org|ninelivesfoundation.org|humanimalconnection.com|bayareacatrescue.org|cap4pets.org|petharbor.com|thenoahcenter.org|homewardpet.org|adopt.maddiesfund.org|animaladoption.com|animaladoptioncenter.org|theshelterpetproject.org|stuffonmymutt.com|aplacetolovedogs.com|petadoptionnetwork.org|petango.com|bayareapetadoptions.org|dpsrescue.org|petsadoption.com|petsearch.animalhumanenm.org|petsmart.com|petfooddirect.com|petco.com|bluebuffalo.com|petfoodexpress.com|petflow.com|wag.com|pet360.com|purina.com|pets.overstock.com|drsfostersmith.com|petsupplies.com|petfooddepot.com|petsuppliesplus.com|petedge.com|petmountain.com|jefferspet.com|jjdog.com|petsugar.com|funnydogsite.com|cuteanimalsonline.com|cutestpaw.com|dailypuppy.com|tehcute.com|thedailycute.com|petoftheday.com|attackofthecute.com|cutearoo.com|cuteoverload.com|cutestpetever.com|justcuteanimals.com|lolcuteanimals.com|reallycutepets.wordpress.com|cutestpetcontest.com|dogfoodadvisor.com|dogfoodanalysis.com|petfoodhonesty.com|pets.webmd.com|petmd.com|dogingtonpost.com|healthypets.mercola.com|entirelypets.com|petage.com|lifewithdogs.tv|pawnation.com|dogtime.com|cesarsway.com|dogs.lovetoknow.com|cats.lovetoknow.com|puppyintraining.com|dogster.com|petsmartcharities.org|1800petmeds.com|petcarerx.com|";
        var insitePagesWhiteList = "sheknows\.com\/pets-and-animals|selectsmart\.com\/DOG|selectsmart\.com\/CAT|today\.com\/pets|peoplepets\.com\/people\/pets|shine\.yahoo\.com\/blogs\/pets|animalplanet\.com\/pets|ellentv\.com\/pets|imgur\.com\/r\/Pets|imgur\.com\/r\/cats|imgur\.com\/r\/dogs|reddit\.com\/r\/aww|tractorsupply\.com\/en\/store\/pet|ebay\.com\/chp\/pet-supplies|ebay\.com\/chp\/Dog-Supplies-\/20742|ebay\.com\/chp\/Cat-Supplies-\/20737|pinterest\.com\/all\/animals|photobucket\.com\/images\/dog?page=1|dogperday\.com\/gallery\/index\.php?cat_id=1|sfbay\.craigslist\.org\/pet|sfbay\.craigslist\.org\/sfc\/pet|sfbay\.craigslist\.org\/sby\/pet|sfbay\.craigslist\.org\/eby\/pet|sfbay\.craigslist\.org\/nby\/pet|sfbay\.craigslist\.org\/scz\/pet|newyork\.craigslist\.org\/pet|losangeles\.craigslist\.org\/pet|chicago\.craigslist\.org\/pet|houston\.craigslist\.org\/pet|philadelphia\.craigslist\.org\/pet|phoenix\.craigslist\.org\/pet|walmart\.com\/cp\/pet-supplies\/5440|petadoption\.ocpetinfo\.com\/Adopt|petstew\.com\/index\.html|sfbay\.craigslist\.org\/pen\/pet|animall\.org\/adopt";
        var carsDomainsWhiteList = "ebay\.com\/sch\/cars-trucks|autos\.yahoo\.com|kbb\.com|edmunds\.com|carmax\.com|copart\.com|feng\.com|truecar\.com|manheim\.com|carsdirect\.com";
        var carsDlsourcesWhiteList = "|test|oldnewtab|coup|fastestchrome|undefined|quickmarkup|pricecompareus|coy|savingsplugin|youtubelightswitch|certifieddeals|willfindinfo|scrolltotopbutton|chromereload|mememanager|specialsavings|diigo|surfcanyon|quiknowledge|cbsdownloadnsave|highlightly|openinstalleravg|cbswxdownload|vbates|shao|cubiezbuddy|genc|cbsclicktosave|svwb|fastestie|ietab|linksicle|smoothscroll|addtofeedly|v9speeddial|pcom|orbitum|mozilla|grts|websuggestors|babylonpilot|chromereload2|calculator|locuspoint|xnotifier|savantmintcast|dynamicpricer|webassistant|picn|w3ic-5|cbsaddictthing|infoatoms|vix|photodownloader|no_dlsource|selectionlinks|baseflash|cbsfastsave|inologix|spbpilot|supershopping|chrome_superfish|tidy2|similarsites|824|discountapp|geniusbox|cupr|facebooknotifications|applianfreecorder|chosa|shch|specialsavingsjs|sfrvzr|cost|mycn|openinstaller|sisa|reimage|smc|ssale|translateselection|thewifiprotector|samag|legendasfree|jobfinder|crossrider|saveas|foodbuzz|2couponmatcher2|bestoffers|svnt|soft32|w3ic-2|specialsavings5|fbskins|svkp|sck|autotranslate|dealally|couponmatcher|fastestfox|babylon|74055d|saven|offerswizard1|vuupc1|lesstabs|irontraffic|sfsa|myshp|rvztubes|specialsavings4|pricecompareus2|fadownloader|dnkp|saveit|windealist|contentexp|driveviewer|btos|smsh|nextaces|neatbookmarks|soft32test|trollemoticons|sfkp|cbsbflix|mailchecker|bit|shopglider|wallpaper|fastestsafari|converter|obronavpn2|setaga|wisestampcoup|geniustranslate|cbsbcool|colormyfb|wbsv|spco|smoothgestures|tidy|pacodeal|vdx|";
        var CPNenabled = "0";
        var enableCpnOnAddBl = parseInt("1");
        var CpnEnabledData = CPNenabled.split('|');
        var WSBL = null; // black list value stays. to avoid double checking.
        var CPNBL = null; // Coupons black list value stays. to avoid double checking.
        var PBL = null;  // Page black list value stays. to avoid double checking.
        var insiteWL = null;
        var insitePageWL = null;
        var carsDomainWhiteList = null;
        var secureDomainsWhiteList = "|google.com|etsy.com|victoriassecret.com|";

        CpnEnabledData[0] = (+CpnEnabledData[0]);
        CpnEnabledData[1] = (+CpnEnabledData[1]);

        if(CpnEnabledData[0] && !enableCpnOnAddBl)
            WSblacklist += WSAddblacklist;

        function isWSBlacklist(){
            if(WSBL === null)
                isBlacklist();
            return WSBL;
        }        

        function isCPNBlacklist(){
            if(CPNBL === null)
                isBlacklist();
            return CPNBL;
        }
        
        function isInsiteWL(){
            if(insiteWL == null)
                isInsiteWhiteList();
            return insiteWL;
        }        

        function isInsitePageWL(){
            if(insitePageWL == null)
                isInsitePageWhiteList();
            return insitePageWL;
        }

        function isBlacklist()
        {
            if(WSBL !== null && CPNBL !== null)
                return; // since we already were inside this.

            var ourHostName = document.location.host;
            var subsHosts;

            if (ourHostName == undefined || ourHostName.length == 0)
            {
                return 0;
            }

            ourHostName = ourHostName.toLowerCase();
            WSBL = false;
            CPNBL = false;

            if (overrideBlackList.indexOf('|'+ourHostName+'|') != -1)
            {
                return;
            }

            subsHosts = ourHostName.replace(/[^.]/g, "").length; // how many time there are "."
            for(var i=0 ; i < subsHosts ; i++) {
                if(WSblacklist.indexOf("|"+ourHostName+"|") != -1){
                    WSBL = true;
                }
                if(CPNblacklist.indexOf(";"+ourHostName+";") != -1){
                    CPNBL = true;
                }
                ourHostName = ourHostName.substring(ourHostName.indexOf(".")+1,ourHostName.length);
            }
        }
        
        function isInsiteWhiteList(){
            if (insiteWL != null) {
                return;
            }
            insiteWL = 0;
            var ourHostName = document.location.host;
            var subsHosts;

            if (ourHostName == undefined || ourHostName.length == 0)
                return 0;

            ourHostName = ourHostName.toLowerCase();
            subsHosts = ourHostName.replace(/[^.]/g, "").length; // how many time there are "."
            for(var i=0 ; i < subsHosts ; i++) {
                if(insiteWhiteList.indexOf("|"+ourHostName+"|") != -1){
                    insiteWL = 1;
                }
                ourHostName = ourHostName.substring(ourHostName.indexOf(".")+1,ourHostName.length);
            }            
        }
        
        function isInsitePageWhiteList(){
            if (insitePageWL != null) {
                return;
            }

            insitePageWL = 0;
            var url = document.location.href;
            var pagesList = new RegExp(insitePagesWhiteList, 'gi');
            var matched = url.match(pagesList);

            if (matched != null && matched.length > 0) {
                insitePageWL = 1;
            }
        }

        function isCarsDomainWhiteList()
        {
            if (carsDomainWhiteList === null && document.location.href.search(new RegExp(carsDomainsWhiteList, 'i')) !== -1 && carsDlsourcesWhiteList.indexOf('|'+similarproducts.b.qsObj.dlsource+'|') !== -1)
            {
                carsDomainWhiteList = 1;
            }

            return carsDomainWhiteList;
        }

        function isInList(list, indication) {
            var ourHostName = document.location.host;
            var subsHosts;

            if (ourHostName == undefined || ourHostName.length == 0)
                return 0;

            ourHostName = ourHostName.toLowerCase();
            subsHosts = ourHostName.replace(/[^.]/g, "").length; // how many time there are "."
            for(var i=0 ; i < subsHosts ; i++) {
                if(WSblacklist.indexOf("|"+ourHostName+"|") != -1){
                    WSBL = true;
                }
                ourHostName = ourHostName.substring(ourHostName.indexOf(".")+1,ourHostName.length);
            }        
        }
        
        function isPageBlacklist() {
            if (PBL == null) {            
                var url = window.location.hostname + (window.location.pathname === '/' ? '' : window.location.pathname);
                if (url == undefined || url.length == 0) {
                    return 0;
                }
                if(pageBlackList.indexOf("|"+url+"|") != -1){
                    PBL = 1;
                }
                else {
                    PBL = 0;
                }                
            }
            return PBL;
        }

        function getCpnData(){
            return CpnEnabledData;
        }

        function getWSBlacklist(){
            return WSblacklist;
        }

        function isSecureDomainWhiteList(domain)
        {
            if (secureDomainsWhiteList.toLowerCase().indexOf('|'+domain.toLowerCase()+'|') !== -1){
                return 1;
            } else {
                return 0;
            }
        }

        return {
            isWSBlacklist : isWSBlacklist,
            isCPNBlacklist : isCPNBlacklist,
            getCpnData : getCpnData,
            isPageBlacklist : isPageBlacklist,
            isInsiteWL: isInsiteWL,
            isInsitePageWL: isInsitePageWL,
            isCarsDomainWhiteList: isCarsDomainWhiteList,
            getWSBlacklist : getWSBlacklist,
            isSecureDomainWhiteList : isSecureDomainWhiteList
        };

    }(),

    getUserHash: function(){
        var nowTime = new Date();
        var chr;
        var userHash = 0 ;
        var str = navigator.userAgent + navigator.platform + screen.height + screen.pixelDepth + screen.width + nowTime.getTimezoneOffset();
        str = str.toLowerCase();
        for (var i = 0; i < str.length; i++) {
            chr = str.charCodeAt(i);
            userHash = ((userHash<<5)-userHash)+chr;
            userHash = userHash & userHash; // Convert to 32bit integer
        }
        if(userHash<0)
            userHash=userHash*(-1);

        return userHash;
    },

    versionManager: versionManager = function(){
        function useNewVer(groupsDivNum ,Sdate, Edate){ // date format 2012.12.30
            if(!groupsDivNum)
                groupsDivNum = 100;
            var totalDays,daysLeft,daysBeen,MyDownloadGroup;
            var groupJumps;
            var nowTime = new Date();

            daysLeft = compareDates(nowTime, Edate);
            if(daysLeft <= 0)
                return true; // new version

            MyDownloadGroup = similarproducts.utilities.getGroup(groupsDivNum);

            totalDays = compareDates(Sdate, Edate);
            daysBeen = compareDates(Sdate, nowTime);
            daysBeen++;

            groupJumps = Math.round(daysBeen*(groupsDivNum/totalDays));

            if(MyDownloadGroup <= groupJumps)
                return true; // use new version
            else
                return false; // use old version
        }

        function compareDates(OD,ND){//od = old date, nd= new date
            if(!OD || !ND){
                return -1;
            }

            var miliDays=1000*60*60*24;
            var dif;
            nowTime = getRealDate(OD);
            NewDate = getRealDate(ND);

            dif=(NewDate.getTime()-nowTime.getTime())/miliDays;
            dif=Math.round(dif);
            return dif;
        }

        function getRealDate(Ldate){ //yyyy.mm.dd
            if(!Ldate){
                return 0;
            }
            if(Ldate.getTime){
                return Ldate;
            }
            var tSp=Ldate.split('.');
            var newTime= new Date();
            newTime.setYear(tSp[0]);
            newTime.setMonth(tSp[1]-1);
            newTime.setDate(tSp[2]);
            return newTime;
        }

        return {
            useNewVer: useNewVer
        };

    }(),

    getGroup: function (groupsDiv){
        var userHash = similarproducts.utilities.getUserHash();
        var usrGroupNumber = (userHash % groupsDiv);
        return(usrGroupNumber);
    },

    abTestUser: function(groupDevision, abTestGroupMax){ // (100 groups get 0-3 of people = (100,3)
        abTestGroupMax--;
        var currGroup = similarproducts.utilities.getGroup(groupDevision);
        return (abTestGroupMax >= currGroup);
    },

    extractDomainName: function(url, method) // the method param is for a/b testing purposes, we will remove it and the additional legacy extraction algorithms later
    {
        var slicedUrl = url.toLowerCase().split('.');
        var length = slicedUrl.length;
        var tldRegex = /^(com|net|info|org|gov|co)$/; //TLD regex

        if (length > 2) // i.e. www.google.com.br, google.co.il, test.example.com
        {
            if (tldRegex.test(slicedUrl[length-2])) // Check second to last part if it passes the TLD regex.
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

    sfWatcher : sfWatcher = function(){
        var isWatcherReady = false;
        if(sfWatch) isWatcherReady = true;

        function complete(str){
            if(isWatcherReady)
                sfWatch.loadingComplete(str);
        }

        function setState(str){
            if(isWatcherReady && str)
                sfWatch.setStage(str);
        }

        function setUserid(str){
            if(isWatcherReady && str)
                sfWatch.setUserid(str);
        }

        function setDlsource(str){
            if(isWatcherReady && str)
                sfWatch.setDlsource(str);
        }

        function setVersion(str){
            if(isWatcherReady && str)
                sfWatch.setVersion(str);
        }

        function setSession(str){
            if(isWatcherReady && str)
                sfWatch.setSession(str);
        }

        function setDlsource(str){
            if(isWatcherReady && str)
                sfWatch.setDlsource(str);
        }

        function setImgurl(str){
            if(isWatcherReady && str)
                sfWatch.setImgurl(str);
        }

        function setExtraData(nm,val){
            if(isWatcherReady && nm && val)
                sfWatch.setExtraData(nm, val);
        }

        function setReInit(){
            if(isWatcherReady)
                sfWatch.init(8000);
        }

        return {
            complete:complete,
            setState:setState,
            setUserid:setUserid,
            setVersion:setVersion,
            setSession:setSession,
            setDlsource:setDlsource,
            setImgurl:setImgurl,
            setArgs:setExtraData,
            reset:setReInit
        }
    }(),

    pricing : prSpl = { 
    prReg: /(([\$\£\€\u20AC]|eur|ils|rs)\.?\s*(<[^>]+>|&nbsp;|\s)?([0-9]+[\,\.])?[0-9]+([\,\.][0-9]+)?)|(([0-9]+[\,\.])?[0-9]+([\,\.][0-9]+)?)(<[^>]+>|&nbsp;|\s|\s\,\-\s)?(€|\u20AC|eur|ils|rs)\.?/ig,
    pageType: '',
    jq:0,
    imgData:{},
    priceFont:0,
    closestPrice:2000,
    factorPrice:150, //factor for distance from the closest.
    elmFound:0,
    stn: function(str) {       // str to num
    	if (!str) {
            return 0;
        }
        var num = /[0-9]/gi;
        var n = str.match(num);
        n = n ? n.join('') : 0;
        n = n.length ? parseInt(n) : 0;
        return n;
    },
    
    split: function(pr) {
        pr = pr.replace(/[\n\t ]/gi, "");
        pr = pr.replace(/<[^>]+>|&nbsp;/i, '');
        pr = pr.replace(/<.+/g, '');
        var r, c, tmp,fullPrice,
            regSign = /[\$\£\u00A3\€\u20AC]|eur|ils|r|rs\.?/gi;
        if (!pr) {
            r = 0;
            c = 0;
            sign = '';
        }
        var sn = pr.match(regSign),
            sign = sn ? sn[0] : '',
            fpr = pr.replace(regSign, '');
        var dot = fpr.indexOf('.'), comma = fpr.indexOf(','), del, delSign;
        if (dot == -1 && comma == -1) {
            r = this.stn(fpr);
            c = 0;
        }
        else {
            del = Math.max(dot, comma);
            delSign = fpr[del] || (dot > comma ? '.' : ',');
            fpr = fpr.replace(/ /g,''); // clear whitespace
            if(fpr.length - del <= 3){ // in case the number was 2.800
                tmp = fpr.split(delSign);
                r = this.stn(tmp[0]);
                c = this.stn(tmp[1]);
            }
            else{
                r = this.stn(fpr);
                c = 0; // must change this !
            }
        }
        fullPrice = parseFloat(r + '.' + c);
        if(r==0 && c==0)
            return 0
        else
            return {
                round: r,
                cents: c,
                sign: sign,
                fullPrice: fullPrice
            };
    },
    
    choise: function(pr, num) {
        num = num == 2 || num == 3 ? num : 2;
        var ind;
        if (pr && pr.length > num - 1) {
            for (var i = 0; i < num; i++) {
                pr[i] = this.split(pr[i]);
            }
            pr.sort(this.cmp);
            if (num == 3 && pr[1].fullPrice == pr[2].fullPrice) {
                ind = 0;
            }
            else if (num == 2 && pr[1].fullPrice/pr[0].fullPrice > 4) {
                ind = 1;
            }
            else {
                ind = num-2;
            }
            return pr[ind].sign + ' ' + pr[ind].fullPrice;
        } 
        else {
            return 0;
        }        
    },
    
    cmp: function (a, b) {
          if (a.fullPrice < b.fullPrice)
             return -1;
          if (a.fullPrice > b.fullPrice)
            return 1;
          return 0;
    },
    
    
    work: function(pr) {
        var dl = [], ar = [], un = [], i, j;
        for (i = 0; i < pr.length; i++) {
            dl[i] = this.getDl(pr[i]);
            if (dl[i].tag == "strike" || dl[i].tag == "s") {
                dl[i].inv = 1;
            }
        }
       for (i = 0; i < dl.length; i++) {
            for(j = i+1; j < dl.length; j++) {
                if(dl[i].tag == dl[j].tag && (dl[i].cl && dl[i].cl == dl[j].cl && dl[i].price != dl[j].price || dl[i].stl && dl[i].stl == dl[j].stl) /* && !dl[i].id */) {
                    j = ++i;
                    dl[i].inv = 1;
                }
            }
            if (!dl[i].inv) {
                ar.push(dl[i]);
            }
        }
        for(i=0; i<ar.length; i++) {
            for(j=i+1; j<ar.length; j++) {
                if (ar[i].price == ar[j].price)
                    j = ++i;
            }
            un.push(ar[i]);
        }
        return un;
    },

    getDl: function(st) {
        st = st.toLowerCase();
        var dl = {
            tag: '',
            price: ''            
        };
        if (st) {
            st = st.replace('< ', '<');
            dl.st = st;
            var tag = st.substring(0, st.indexOf('>')+1);
            dl.tag = tag.substring(st.indexOf('<')+1, st.indexOf(' '));
            var tmp = st.match(this.prReg);
            if (tmp && tmp[0]) {
                dl.price = tmp[0];
            }
            if (st.match(/(\([0-9]+%\))|from|sfprice/gi)) {
                dl.inv = 1;
                return dl;
            }
            var icl = tag.indexOf('class');
            if (icl > -1) {
                dl.cl = tag.substring(icl+5, tag.length-1).match(/[\"\'][A-Za-z0-9\s-_]+[\"\']/i);
                if (dl.cl) {
                    dl.cl = dl.cl[0].replace(/[\"\']/g, '');
                }
            }
            icl = tag.indexOf('style');
            if (icl > -1) {
                dl.stl = tag.substring(icl+5, tag.length-1).match(/[\"\'][^\"\']+[\"\']/);
                if (dl.stl) {
                    dl.stl = dl.stl[0].replace(/[\"\']/g, '');
                }
                
            }
            if (tag.indexOf('id')>-1) {
                dl.id = 1;
            }
        }
        return dl;
    },
    
    extr: function(str) {
        var pr = str.match(this.prReg);
        if (pr && pr[0]) {
            return pr[0];
        }
        else {
            pr = this.split(str);
            if (pr) {
                return pr.sign + ' ' + pr.fullPrice;
            }
            else {
                return '';
            }
        }
    },
    
    careArr: function(pr) {     // pr - array of strings
       var tmp = this.work(pr); // tmp - array of objects {st, tag, price, cl, stl}
       if (tmp && tmp[0]) {                                       
           var fpr = this.extr(tmp[0].st);                                   
           if(tmp.length == 1) {
              return fpr; 
           }
           else if(tmp.length == 2) {
               return this.choise([tmp[0].st, tmp[1].st], 2); 
           }
           else if(tmp.length == 3){
               return this.choise([tmp[0].st, tmp[1].st, tmp[2].st], 3); 
           }
           else {
               return 0;
           }
       }
       else {
           return 0;
       }
    },
    
    get: function(im) {
        similarproducts.utilities.sfWatcher.setState("getPrice");
        if (im) {
            var tx;
            var i = 0;
            var pr;
            for(var nd = im.parentNode; nd && nd.nodeName != "BODY"; nd = nd.parentNode) {         
                i++;
                tx = nd.innerHTML;
                if (tx) {
                    tx = tx.replace(/[\n\t]/gi, "");
                    tx = tx.replace(/\s+/g, " ");
                    pr = tx.match(/((from:?(&nbsp;|\s))?<[^\/>]+>\s*(now|sale|us|au|r)?\s*([\$\£\€\u20AC]|usd|eur|ils|rs)\.?\s*(<[^>]+>|&nbsp;|\s)?\s*([0-9]+[\,\.])?[0-9]+([\,\.][0-9]+)?(<[^>]+>[^<]+<\/[^>]+>)?)(&nbsp;|\s)?(\([0-9]+%\))?|((from:?(&nbsp;|\s))?<[^\/>]+>\s*?(([0-9]+[\,\.])?[0-9]+([\,\.][0-9]+)?)(<[^>]+>|&nbsp;|\s|\s\,\-\s)?(€|usd|eur|ils|rs)\.?(&nbsp;|\s)?(\([0-9]+%\))?)/ig);
                    if (pr && pr.length) {
                        return this.massGet(nd,im,pr);
                        break;

                    }
                }
            }
        }
        return 0;
    },

    massGet: function(pnd,im,elm){ //pnd - parentObject, img - our image, elm - array of elements with tag.
        this.priceFont = 0;
        this.closestPrice = 1000;
        this.elmFound=0;
        var clsArr=[];
        var idArr=[];
        var tagArr=[];
        var nodesList=[];
        var cutLoop=false;
        var imgData;
        var t = this;

        var Id_Regex=/(id\s?=\s?")(.*?)(?=\")/ig;
        var Class_Regex=/(class\s?=\s?")(.*?)(?=\")/ig;
        var Tag_Regex=/(<)([^>^/^\s]+)/ig;

        if(typeof(spsupport) != "undefined") this.jq = spsupport.p.$;
        else{
            if(typeof(sfPageAnalyzer) != "undefined")
                this.jq = sfPageAnalyzer.$;
            else{
                return 0;
            }
        }

        // get the elements to array of classes and array of ID
        for(var i=0;i<elm.length;i++){
            elm[i].replace(Id_Regex,function($0,$1,$2){
                if($2!=""){
                    $2 = t.trimPrices($2);
                    if(idArr[$2])idArr[$2]++;
                    else idArr[$2]=1;
                    cutLoop = true;
                }
            });

            if(cutLoop){ // preventing testing the classes regex
                cutLoop=false;
                continue; // no need to check of class
            }

            elm[i].replace(Class_Regex,function($0,$1,$2){
                if($2!=""){
                    $2 = t.trimPrices($2);
                    if(clsArr[$2])clsArr[$2]++;
                    else clsArr[$2]=1;
                    cutLoop = true;
                }
            });

            if(cutLoop){ // preventing testing the tag regex
                cutLoop=false;
                continue; // no need to check of class
            }
            elm[i].replace(Tag_Regex,function($0,$1,$2){
                if($2!=""){
                    $2 = t.trimPrices($2);
                    if(tagArr[$2])tagArr[$2]++;
                    else tagArr[$2]=1;
                }
            });
        }

        this.imgData = this.nodePos(im);

        // now get unique items
        nodesList=nodesList.concat(this.getUniqueNodes(pnd,idArr,1,1));
        nodesList=nodesList.concat(this.getUniqueNodes(pnd,clsArr,3,2));
        nodesList=nodesList.concat(this.getUniqueNodes(pnd,tagArr,3,3));
        if(nodesList.length==0){ // i couldn't find the objects.. use other functions
            if(elm.length < 6 && this.elmFound == 0) {
                if(elm.length == 1)
                    return this.extr(elm[0]);
                else
                    return this.careArr(elm);
            }
            else{
                return 0;
            }
        }

        nodesList.sort(this.distSort); // sorting by distance;
        finalPrice = this.decideRealPrice(nodesList);
        return finalPrice;
    },

    trimPrices: function(str){
        return str.replace(/^\s+|\s+$/g, '');
    },

    decideRealPrice: function(obs){ // obs - array of element that contains the price
        var obL = obs.length;

        for(var i = 0; i< obL; i++){
            it = obs[i];
            if(it.dis>this.factorPrice+this.closestPrice){
                return 0;
                break;
            }

            if(it.fsize == this.priceFont && it.fdeco != 'line-through'){
                return this.getPriceFromObj(it);
                break;
            }
        }
        return 0;
    },

    getPriceFromObj:function(ob){
        var str = this.extr(ob.innerHTML);
        var price = this.split(str);
        return price.sign+price.fullPrice;
    },

    distSort: function(a,b){
        if (a.dis < b.dis)
            return -1;
        if (a.dis > b.dis)
            return 1;
        return 0;
    },

    nodePos: function(nd) { //nd - node
        if(typeof(nd)!=='object'){
            return 0;
        }

        var jqImg =  this.jq(nd);
        var imgPos = jqImg.position(),
            imgOffset = jqImg.offset();

        return {
            l: imgPos.left,
            t: imgPos.top,
            w: jqImg.outerWidth(true),
            h: jqImg.outerHeight(true),
            x: imgOffset.left,
            y: imgOffset.top
        };
    },

    getUniqueNodes:function(pnd,arr,mx,fb){  // pnd - parent node, arr - the array, mx - maximum of occurence,findBy
        var ans=[];
        var objs;
        var tmpObj; // for temp usage.
        var maxDisX = 700; // max distance X
        var maxDisY = 200; // max distance X
        var findBy, dis, moveX, moveY; //dis = total distance, moveX - deference X, moveY - deference Y

        switch(fb){
            case 1:
                findBy = "#";
                break;
            case 2:
                findBy = ".";
                break;
            case 3:
                findBy = "";
                break;
            default:
                return ans;
        }

        maxDisX = maxDisX + this.imgData.w;
        maxDisY = maxDisY + this.imgData.h;

        for (var item in arr ){
            if(arr[item] && arr[item]<=mx){
                item = this.trimPrices(item);
                if(!item || item==""){ // if it's empty somehow;
                    continue;
                }
                objs = this.jq(pnd).find(findBy+item.replace(/[!"#$%&'()*+,.\/:;<=>?@[\\\]^`{|}~]/g, '\\\$&').replace(/ /g,findBy));
                if(objs.length<=mx){
                    for(var i=0;i<objs.length;i++){
                        if(typeof(objs[i])==='object'){
                            // i found the item. now, i check it's positions.
                            tmpObj = objs[i];
                            tmpObj.po = this.nodePos(tmpObj); // get it's position;
                            if (tmpObj.po) {
                                moveX = tmpObj.po.x - this.imgData.x;
                                moveY = tmpObj.po.y - this.imgData.y;
                                if(moveX<0)moveX *= (-1);
                                if(moveY<0)moveY *= (-1);

                                if(moveX<maxDisX && moveY<maxDisY){ // item is in a good position. add
                                    tmpObj.dis = parseInt(moveX+moveY);
                                    tmpObj.fsize = parseInt(this.zxcSVInt(tmpObj,'font-Size'));
                                    tmpObj.fdeco = this.zxcSVInt(tmpObj,'text-Decoration');

                                    if(tmpObj.fsize > this.priceFont && tmpObj.fdeco !='line-through')
                                        this.priceFont = tmpObj.fsize;

                                    if(this.closestPrice>tmpObj.dis && tmpObj.fdeco !='line-through')
                                        this.closestPrice = tmpObj.dis;

                                    ans.push(tmpObj);
                                }
                                else{
                                    this.elmFound = 1;// flag.. don't search with other method
                                }
                            }
                        }
                    }
                }
            }
        }
        return(ans);
    },

    zxcSVInt: function (obj,par){ // obj - object, par - style parameter we want.
    	if (obj.currentStyle) return obj.currentStyle[par.replace(/-/g,'')];
    	return document.defaultView.getComputedStyle(obj,null).getPropertyValue(par.toLowerCase());
    }
} ,
    abTestUtil: (function() {
    var names = [
        "tier1_bucket",
        "tier1_curr_group",
        "tier1_next_group",
        "tier1_prev_group",
        "tier2_bucket",
        "tier2_curr_group",
        "tier2_next_group",
        "tier2_prev_group"
    ];
    var dataObj = {};
    var dataString = "";

    function init() {
        if (dataObj === {}) {
            for (var i = 0; i < names.length; i++) {
                dataObj[names[i]] = "";
            }
        }
    }
    
    function setValues(obj) {
        if (obj) {
            if (obj.tier1) {
                dataObj.tier1_bucket = obj.tier1.bucket || "";
                dataObj.tier1_curr_group = obj.tier1.group;
                dataObj.tier1_next_group = obj.tier1.nextGroup;
                dataObj.tier1_prev_group = obj.tier1.previousGroup;
            }
            if (obj.tier2) {
                dataObj.tier2_bucket = obj.tier2.bucket || "";
                dataObj.tier2_curr_group = obj.tier2.group;
                dataObj.tier2_next_group = obj.tier2.nextGroup;
                dataObj.tier2_prev_group = obj.tier2.previousGroup;
            }            
        }
        dataString = '';
        for (var i = 0; i < names.length; i++) {
            if (dataObj[names[i]] == null || typeof(dataObj[names[i]]) === "undefined") {
                dataObj[names[i]] = "";
            }
            dataString += ("&" + names[i] + "=" + dataObj[names[i]]);
        }
    }
    
    function setValuesFromQS(qsObj) {
        if (qsObj) {
            dataString = '';
            for (var i = 0; i < names.length; i++) {
                if (qsObj[names[i]] == null || typeof(qsObj[names[i]]) === "undefined") {
                    dataObj[names[i]] = "";
                }
                else {
                    dataObj[names[i]] = qsObj[names[i]];
                }
                dataString += ("&" + names[i] + "=" + dataObj[names[i]]);
            }
        }
    }
    
    
//    function overrideBucket() {
//        dataObj.tier1_bucket = ((similarproducts.b.qsObj && similarproducts.b.qsObj.bucket ) || (window.qsObj && window.qsObj.bucket )) || dataObj.tier1.bucket;
//        dataObj.tier2_bucket = ((similarproducts.b.qsObj && similarproducts.b.qsObj.bucket ) || (window.qsObj && window.qsObj.bucket )) || dataObj.tier2.bucket;
//    }
    
    function getBucket() {
        return dataObj.tier1_bucket || dataObj.tier2_bucket;
    }
    
    function addDataToObject(obj) {
        if (!obj) {
            obj = {};
        }
        for (var i = 0; i < names.length; i++) {
            obj[names[i]] = dataObj[names[i]];
        }
        return obj;
    }

    function getDataString() {
        return dataString;
    }
    
    init();
    
    return {
        setValues: setValues,
        setValuesFromQS: setValuesFromQS,        
        getBucket: getBucket,
        addDataToObject: addDataToObject,
        getDataString:getDataString
    };
    
})()
};
        spsupport = {
            log : function(){if (window.console) {for(var i in arguments) {console.log(arguments[i]);}}},
            dtBr: dtBr = function(matchStr) {
    var ua = navigator.userAgent;
    var br = "unknown";
    if (ua) {ua = ua.toLowerCase();if (ua.indexOf("msie 7") > -1){br = "ie7";}
    else if (ua.indexOf("msie 8") > -1) {br = "ie8";}
    else if (ua.indexOf("msie 9") > -1) {br = "ie9";}
    else if (ua.indexOf("msie 10") > -1 ) {br = "ie10"}
    else if (ua.indexOf("firefox/5") > -1) {br = "ff5";}
    else if (ua.indexOf("firefox/6") > -1) {br = "ff6";}
    else if (ua.indexOf("firefox/7") > -1) {br = "ff7";}
    else if (ua.indexOf("firefox/8") > -1) {br = "ff8";}
    else if (ua.indexOf("firefox/9") > -1) {br = "ff9";}
    else if (ua.indexOf("firefox/10") > -1) {br = "ff10";}
    else if (ua.indexOf("firefox") > -1) {br = "ff";}
    else if (ua.indexOf("chrome") > -1) {br = "ch";}
    else if (ua.indexOf("apple") > -1) {br = "sa";}}
    return matchStr ? br.indexOf(matchStr) > 0 : br;
}

        };
        if( window.location.href.indexOf( "amazon.com/" ) > 0 && window.location.href.indexOf( "/search/" ) > 0 && window.location.href.indexOf( "#sf" ) > 0 ){
            window.location.replace( window.location.href.substring( 0, window.location.href.indexOf( "#sf" ) ) );
        }
        spsupport.b = {};
        spsupport.br = { isIE7: (navigator.userAgent.toLowerCase().indexOf("msie 7") > -1)};

        similarproducts.b.// This function extracts key value data from the selected querystring obj
// Input parameters:
// obj_to_add = the desired object to add the querystring object into
// initial_QS the querystring value

setQueryString = function(obj_to_add,initial_QS){
    obj_to_add.qsObj={};
    initial_QS.replace(
        new RegExp("([^?=&]+)(=([^&]*))?", "g"),
        function($0, $1, $2, $3)
        {
            if ($1 === 'ctid')
            {
	            $1 = 'CTID';
            }

	        obj_to_add.qsObj[$1] = decodeURIComponent($3);
        }
    );
}

        similarproducts.b.setQueryString(similarproducts.b, similarproducts.b.initialQS);

        var currentVersion = "13.1.4.63";
        if(similarproducts.b.qsObj.ver && similarproducts.b.qsObj.ver !== currentVersion){
            similarproducts.b.initialQS = similarproducts.b.initialQS.replace(similarproducts.b.qsObj.ver, currentVersion);
            similarproducts.b.qsObj.ver = currentVersion;
        }

        if (!similarproducts.b.qsObj.statsReporter) {
            similarproducts.b.qsObj.statsReporter = "true";
        }
        if (!similarproducts.b.qsObj.dlsource) {
            similarproducts.b.qsObj.dlsource = "qomciru";
        }

        try
        {
            var request;
            var sampleUserTrafficRnd = Math.floor(Math.random() * 9999);
            if ( sampleUserTrafficRnd == 1)
            {
                request = new Image();
                request.src = 'https://www.superfish.com/ws/trackSession.action?action=sample-user-traffic&dlsource='+similarproducts.b.qsObj.dlsource+'&page_url=' + encodeURIComponent(document.location.href);
            }
        }
        catch(e){}

        similarproducts.b.qsObj.userid = similarproducts.b.qsObj.userid || "";
        similarproducts.b.qsObj.userId = similarproducts.b.qsObj.userId || "";
        if (similarproducts.b.qsObj.dlsource == "pagetweak" && similarproducts.b.qsObj.userid == "") {
              similarproducts.b.qsObj.userid = similarproducts.b.qsObj.dlsource;
        }
        
        similarproducts.b.qsObj.userid = 'NTBC' + similarproducts.b.qsObj.userid + 'NTBC';
        similarproducts.b.qsObj.userId = 'NTBC' + similarproducts.b.qsObj.userId + 'NTBC';
        
        similarproducts.b.qsObj.CTID = (similarproducts.b.qsObj.CTID == "null" ? "" : (similarproducts.b.qsObj.CTID || ""));
        
        spsupport.isShowConduitWinFirstTimeIcons = similarproducts.b.qsObj.isShowConduitWinFirstTimeIcons == "true" || similarproducts.b.qsObj.isShowConduitWinFirstTimeIcons == "1";

        similarproducts.b.userData = {};
        similarproducts.userData = (function() {
    
    var sfDomain = '';
    var xdMsgDelimiter;
    var iframe = {};
    var endCallback = function() {};
    var msgCallback = function() {};
    var timer = 0;
    
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

    function createIframe(domain, callback, delimeter) {
        similarproducts.b.userData = {};
        timer = setTimeout(function() {
            var data = {
                "uc":"US",
                "ut":{
                    "tier1":{
                        "bucket": null,
                        "group": -4,
                        "nextGroup": -4,
                        "previousGroup": -4
                    },
                    "tier2":{
                        "bucket": null,
                        "group": -4,
                        "nextGroup": -4,
                        "previousGroup": -4
                    },
                "userId": similarproducts.b.qsObj.userid
                },
                "needToShowOptOut": false
            };
            setABData(data);
            callback();
            if(Math.floor(Math.random() * 1000) == 1) {
                var url = sfDomain + "trackSession.action?userid=" + similarproducts.b.qsObj.userid + "&sessionid=-10&action=ud_host_failed";
                if (typeof(similarproducts.utilities.abTestUtil) != 'undefined' && similarproducts.utilities.abTestUtil && similarproducts.utilities.abTestUtil.getDataString) {
                    url += similarproducts.utilities.abTestUtil.getDataString();
                }            
                var img = new Image();
                img.src = url;     
             }
        }, 5000);
        sfDomain = domain;
        sfDomain = window.location.protocol == "http:" ? sfDomain.replace('https:', 'http:') : sfDomain;
        xdMsgDelimiter = delimeter;
        xdmsg.init(gotMessage);
        iframe = document.createElement('iframe');
        iframe.src = getIframeSrc();
        iframe.setAttribute("style", "position:absolute; top:-100px; left:-100px; z-index: -10; border: none; visibility: hidden;");
        iframe.style.width = "1px";
        iframe.style.height = "1px";
        document.body.appendChild(iframe);
        if (callback && typeof(callback) === 'function') {
            endCallback = callback;
        }
    }
        
    function getIframeSrc() {
        var q = [],
        path = sfDomain + "userData.jsp?";
        if (similarproducts && similarproducts.b && similarproducts.b.qsObj) {
            q.push("dlsource=" + similarproducts.b.qsObj.dlsource);
            q.push("userid=" + similarproducts.b.qsObj.userid);
            q.push("ver=" + similarproducts.b.qsObj.ver);
        }
        return path + q.join('&');
    }
    
    function setData(dataString) {
        var data = JSON.parse(dataString);
        if (similarproducts.utilities.abTestUtil && data.ut && similarproducts.b.userData) {
            similarproducts.b.qsObj.userid = data.ut.userId;
            similarproducts.b.qsObj.userId = data.ut.userId;
            similarproducts.b.initialQS = similarproducts.b.initialQS.replace(/userid=([^&]*)?/gi, 'userid=' + data.ut.userId); 
            setABData(data);
        }
    }
    
    function setABData (data) {
        if (similarproducts.utilities.abTestUtil && data.ut && similarproducts.b.userData) {
            similarproducts.b.userData.uc = (similarproducts.b.qsObj && similarproducts.b.qsObj.country ) || data.uc;
            similarproducts.b.userData.uc = similarproducts.b.userData.uc.toUpperCase();
            similarproducts.b.userData.ut = data.ut;
            similarproducts.b.userData.ut.tier1.bucket = (similarproducts.b.qsObj && similarproducts.b.qsObj.bucket) || similarproducts.b.userData.ut.tier1.bucket;
            similarproducts.b.userData.ut.tier2.bucket = (similarproducts.b.qsObj && similarproducts.b.qsObj.bucket) || similarproducts.b.userData.ut.tier2.bucket;            
            similarproducts.utilities.abTestUtil.setValues(similarproducts.b.userData.ut);
        }
        similarproducts.b.userData.needToShowOptOut = data.needToShowOptOut || false;
    }
            
    function gotMessage(msg, from) {
        if(from && from.indexOf(sfDomain.split('/')[0]) == -1 ) {
            return;
        }

        if (!msg) {
            return;
        }
        
        msg = msg + "";
        var prep = msg.split(xdMsgDelimiter);
        if (+prep[0] == -3344) {
            clearTimeout(timer);
            setData(prep[1]);
            endCallback();
        }
    }
    
	function postToIframe(data)
	{ 
		try{
			var cW = iframe.contentWindow;
			if (cW != top)
			{
				xdmsg.postMsg(cW, data);
			}
		}
		catch(e){}
	} 
    
    return {
        createIframe: createIframe
    };
    
})();


        var sfCodeAdditionalParams = '';
        if (!similarproducts.utilities.blacklistHandler.isWSBlacklist() && !similarproducts.utilities.blacklistHandler.isPageBlacklist() || !similarproducts.utilities.blacklistHandler.isCPNBlacklist() || similarproducts.utilities.blacklistHandler.isCarsDomainWhiteList()) {
            similarproducts.userData.createIframe("https://www.superfish.com/ws/", function() {
                if( ((similarproducts.b.cacheBySubDlsource || "") === "1") &&
                    ((similarproducts.b.qsObj.CTID || "")  !== "")) {
                    sfCodeAdditionalParams = "&mc=" + similarproducts.b.qsObj.CTID;
                } else {
                    if((similarproducts.b.qsObj.partnername || "") !== ""){
                        if((similarproducts.b.qsObj.CTID || "")  !== ""){
                            sfCodeAdditionalParams = "&mc=" + similarproducts.b.qsObj.CTID;
                        } else {
                            sfCodeAdditionalParams = "&mc=" + similarproducts.b.qsObj.partnername.replace(/ /g,"_");
                        }
                    }
                }
                similarproducts.b.inj(window.document, "https://www.superfish.com/ws/sf_code.jsp" + similarproducts.b.initialQS + sfCodeAdditionalParams,1)
            }, "*sfxd*");
        }
        else{
            similarproducts.utilities.sfWatcher.complete("is blacklist");
            if (typeof window.similarProductsNoSearch == "function")
                window.similarProductsNoSearch('{"message":"is blacklist"}');
        }
        similarproducts.partner = {};

similarproducts.partner.init = function() {
    if (this._init) { this._init(); }
};

similarproducts.partner.logoClick = function() {
    if (this._logoClick) { this._logoClick(); }
};

        similarproducts.publisher = {};
similarproducts.publisher.reqCount = 0;
similarproducts.publisher.valCount = 0;
similarproducts.publisher.imgs = [];
similarproducts.publisher.limit = 0;

similarproducts.publisher.init = function() {
    if (this._init) {
        this._init();
    }
};

similarproducts.publisher.pushImg = function(img) {
    var cond = (spsupport.whiteStage.rv || spsupport.whiteStage.st || (similarproducts.sg && similarproducts.sg.sSite ? true : this.imgs.length < this.limit));
    // var cond = this.imgs.length < this.limit;
    if(similarproducts.b.multiImg && cond){
        this.imgs[ this.imgs.length ] = img;
        if( !this.reqCount ){
            this.send();
        }
    }
};

similarproducts.publisher.send = function() {
 //   spsupport.log("send " + this.reqCount + "  " + this.limit + "  " +  this.valCount +  "  " + this.imgs.length);
    if (similarproducts.b.multiImg && this.reqCount < this.limit && this.valCount < this.imgs.length) {
        var im = this.imgs[this.valCount];
        var imgPos = spsupport.api.getImagePosition(im);
        var val = spsupport.api.validateSU(im, parseInt(imgPos.y + im.height - 45));
        // spsupport.log("validate = " + val);
        this.reqCount += val;
        this.valCount++;
    }
    else {
        // similarproducts.util.bCloseEvent( document.getElementById("SF_CloseButton"), 2 );
        spsupport.p.prodPage.e = 1;
    }
};

similarproducts.publisher.fixSuPos = function(top) {
    return (this._fixSuPos ? this._fixSuPos(top) : top);
};

similarproducts.publisher.report = function(action) {
    if (this._report) {
        this._report(action);
    }
};

similarproducts.publisher.extractTxt = function(img) {
    if (this._extractTxt) {
        return this._extractTxt(img);
    }
    else {
        return '';
    }
};



        spsupport.events = function() {
    /*private properties:*/
    var stack = [];
    var maxStackSize = 2500;
    var dom, $;
    var defaultLevel = "info";
    var defaultSource = "main";
    var domId = "SF_EVENTS_LOG";
    var printBuffer = []; 
    var levels = ['all', 'app', 'search', 'info'];
    var defaultFilter = 'all';
    var prefCookie = 'sf_events_log_prefs';
    var activeFilter;
    
    //dom pointers:
    var filters;
    var entries;
    var logHtml = "<div id='"+domId+"'>" + //"<span id='tools-btn' />" +
                      "<div id='sf-header-wrap'>"+
                          "<div class='sfheader'>SimilarProducts Events Log</div>" +
                          "<div id='filters'><span class='label'>Filter By:</span></span><span id='free'><input class='filter' type='text'/><span id='clear'>X</span></span></div>" +
                          "<table cellspacing='0' cellpadding='0' id='entries-header'><tbody><tr>" + 
                                "<th class='label time'><span>Time</span></th>" +
                                "<th class='label level'><span>Level</span></th>" +
                                "<th class='label text'><span>Event</span></th>" +
                                "<th class='label source'><span>Source</span></th>" +
                                "<th class='label url'><span>click to open</span></th>" +
                          "</tr></tbody></table>" +
                      "</div>" +
                      "<div id='entries-wrap'><table cellspacing='0' cellpadding='0' id='entries'><tbody></tbody></table></div>" +
                  "</div>";
    var entryHtml = "<tr class='entry'>" + 
                        "<td class='label time'><span></span></td>" +
                        "<td class='label level'><span></span></td>" +
                        "<td class='label text'><span></span></td>" +
                        "<td class='label source'><span></span></td>" +
                        "<td class='label url' ><a target='_blank' href='#'>click to open</a></td></tr>";
    var urlBox = "<div id='url-box'><div><span class='pointer'></span><a target='_blank' href='' id='api'>open api service<a href='' target='_blank' id='search'>open search service</a></div></div>";
    
    
    /*private methods:*/
    function addLevel(a) {
        levels.push(a);
    }
    
    function isNewLevel(a) {
        if(a) {
            for(var i = 0; i < levels.length; i++) {
                if(a == levels[i]) return false;
            }
            return true;
        }
        return false;
    }
    
    //create a new entry, add it to the stack and print it.
    //urlParams may be href string or json object
    function log(msg, level, source, urlParams) {
        if (similarproducts.b && similarproducts.b.qsObj && similarproducts.b.qsObj.sflog == "-999") {
        level = level ? level.toLowerCase() : null;
        if(stack.length >= maxStackSize) stack.shift();
        if(isNewLevel(level)) addLevel(level);
        var ts = new Date();
        
        var entry = {
            time: ts,
            timeString: getTimeLabel(ts), 
            text: msg.text || msg,
            level: level || defaultLevel,
            source: source || defaultSource,
            url: urlParams,
            index: stack.length
        };
        
        stack.push(entry);
        print(entry);
        return true;
        }
        else {
            return false;
        }
    }
    
    function print(entry) {
        //if print is called b4 jquery loaded, add event to print buffer.
        if(typeof $ !== 'function') {
            printBuffer.push(entry);
        }
        else {
            if(printBuffer.length > 0) {
                for(var i = 0 ; i < printBuffer.length; i++) {
                    addEventRow(printBuffer.pop());
                }
            }
            addEventRow(entry);
        }
    }
    
    function addEventRow(entry) {
        var row = $(entryHtml);        
        if(activeFilter == 'all' || activeFilter === entry.level) $(row).show();    
        else $(row).hide();            
        $(row).addClass(entry.level);
        $(row).prop('id', entry.index);
        if(stack.length%2 === 0) $(row).addClass('alt');
        $('.time span',row).text(entry.timeString);
        $('.level span', row).text(entry.level.toUpperCase());
        $('.text span', row).text(entry.text);
        $('.source span', row).text(entry.source);
        if(entry.url) {
            if(entry.level !== 'search') {
                $('.url a', row).attr('href', entry.url);
            }
            $('.url a', row).css('display', 'block');
        }        
        $(entries).append(row);
        $(entries).parent().prop({ scrollTop: $(entries).parent().prop("scrollHeight") });
    }
    
    function setFilter(level) {
        activeFilter = level;
        if(level == 'all') $('.entry', entries).show();
        else {
            $('.entry:not(.'+level.toLowerCase()+')', entries).hide();
            $('.entry.'+level.toLowerCase(), entries).show();   
        }
    }
    
    function setPreferences(p){
        var val = p;
        var exp=new Date();
        exp.setDate(exp.getDate() + 365);
        val = escape(val) + ((exp==null) ? "" : "; expires="+exp.toUTCString());
        document.cookie = prefCookie + "=" + val;
    }
    
    function getPreferences()
    {
        var i,x,y,jar =document.cookie.split(";");
        for (i = 0; i < jar.length; i++) {
            x = jar[i].substr(0,jar[i].indexOf("="));
            x = x.replace(/^\s+|\s+$/g,"");
            
            if (x == prefCookie) {
                y = jar[i].substr(jar[i].indexOf("=")+1);
                return unescape(y);
            }   
        }
        return false;
    }
    
    function getTimeLabel(ts) {
        var h = ts.getHours().toString();
        var m = ts.getMinutes().toString();
        var s = ts.getSeconds().toString();
        var ms = ts.getMilliseconds().toString();        
        return h + ":" + (m.length > 1 ? m : "0"+m) + ":" + (s.length > 1 ? s : "0"+s)  + ":" + (ms.length > 2 ? ms : (ms.length > 1 ? "0"+ms : "00"+ms));
    }
        
    function togglefilters(a) {
        if(a == 'show' || parseInt($(filters).css('height'),10) == 0) {
            $(filters).animate({height:50}, 200);
        } 
        else {
            $(filters).animate({height:0}, 200);
        }
    }
    
    function exportStack(level) {
        if(level && level.length > 0) {
            var items = [];
            for(var i = 0; i < stack.length; i++) {
                if(stack[i].level == level) items.push(stack[i]);
            }
            return items;
        }
        return stack;
    }
    
    function openUrl(a, params) {
        try {
            params = $.parseJSON(params);
        }
        catch(e) {
            params = 0;
            log('error parsing JSON params for openUrl()', 'error');
        }
        if(params) {
            var pos = $(a).position();
            $(urlBox).css('left', parseInt(pos.left, 10)-200);
            $(urlBox).css('top', parseInt(pos.top, 10)+20);
            $('#api', urlBox).attr('href', buildApiUrl(params));
            $('#search', urlBox).attr('href', buildSearchUrl(params));
            $(a).parent().append(urlBox);
            $(urlBox).show();
        }
    }

    function buildApiUrl(p){
        
        return "http://ia1-p:10009/apiservice/debug?"+ 
            "runtype=byParams"+
            "&session="+p.sessionid+
            "&imageUrl="+p.imageURL+
            "&imageTitle="+p.imageTitle+
            "&documentTitle="+p.documentTitle+
            "&imageRelatedTitle="+p.imageRelatedText+
            "&productUrl="+p.productUrl+
            "&merchantName="+p.merchantName+
            //"&partnerID=&imageType=&sourceId=&identicalOnly=&country=&minimumQuality=
            "&pageType="+p.pageType; //&presentationType=&siteOrigin=&cardPriorities=";
    }
    
    function buildSearchUrl(p){
        return "http://wa1-p:7004/Search/debug?"+
            "session="+p.sessionid+
            "&requestType=byUrl"+
            "&url="+p.imageURL+
            "&imageTitle="+p.imageTitle+
            "&documentTitle="+p.documentTitle+
            "&imageRelatedText="+p.imageRelatedText+
            "&pageUrl="+window.location.href; //&partnerId=&country=";
    }
          
    function init(jquery) {
        if(!dom) {
            $ = jquery;
            dom = $(logHtml);
            $('head').append('<link rel="stylesheet" type="text/css" href="'+similarproducts.b.site+'css/events_log.css">');
            $('body').append(dom);
            filters = $('#filters', dom);
            entries = $('#entries', dom);
            urlBox = $(urlBox);
            for(var i = 0; i < levels.length; i++){
                $('#free', filters).before("<button class='filter' type='button' value='"+levels[i]+"'>"+levels[i]+"</button>")
            }            
            //bind ui events:
            $('button.filter', filters).click(function() {
                $('button.filter', filters).removeClass('active');
                $(this).addClass('active');
                var filter = $(this).text();
                setFilter(filter);
                setPreferences(filter);
            });            
            $('#clear', filters).click(function() {
                $('input[type="text"]', filters).val('').focus();                
            });            
            $('input[type="text"]', filters).keyup(function(e) {
                if($(this).val() == '') setFilter('all');
                else {
                    for(var i = 0; i < levels.length; i++) {
                        if($(this).val() == levels[i]) {
                            setFilter(levels[i]);
                        }
                    }
                }
            });

            $(entries).on('click', '.search .url a', function(e) {
               var entryId = $(this).closest('tr').prop('id');
               var entry = stack[entryId];
               openUrl(this, entry.url); //entry.url is params json               
               if(e.preventDefault) e.preventDefault();
               else e.returnValue = false;
               return false;
            });
            
            $("#entries-wrap", dom).scroll(function(){
                $("#url-box", dom).hide();
            });
            
            var filter = getPreferences();
            if(filter) {
                $('button.filter[value="'+filter+'"]', filters).click();
                setFilter(filter);
            }
            else {
                $('button.filter[value="'+defaultFilter+'"]', filters).click();
            }
            log('log ready'); 
        }
    }
    
    return {
        init: init,
        reportEvent: log,
        get: exportStack
    };
    
}();

var originalJq = window.jQuery,
correctJqVersion = originalJq ? originalJq.fn.jquery === '1.7.1' : false;
if (similarproducts.b && similarproducts.b.qsObj && similarproducts.b.qsObj.sflog == "-999") {
    if(!correctJqVersion) {
        yepnope([{
            load: "http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js?111",
            callback: function(){
                (function() {
                    spsupport.events.init(jQuery);
                })();
            }
        }]);
    }
    else {
        spsupport.events.init(jQuery);
    }
}
        
		}

}());
