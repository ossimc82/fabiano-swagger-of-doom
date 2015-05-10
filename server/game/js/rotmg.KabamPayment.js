var rotmg = rotmg || {};

rotmg.KabamPayment = (function($) {
  var SERVER_INFO_ = {
    "testing": {
      "server": "payv2beta.kabam.com",
      "gameId": 24
    },
    "production": {
      "server": "payv2.kabam.com",
      "gameId": 24
    }
  };

  var apiOptions_ = null;
  var cssPath_ = "";
  var paymentWallInstance_ = null;
  var protocol_ = ('https:' == document.location.protocol ? 'https://' : 'http://');
  var serverInfo_ = null;

  var diaplayBeginnersPackagePayment_ = function() {
    displayPaymentWall_({"discount_type": "beginner"})
  };

  var displayPaymentWall_ = function(extraOptions) {
    $(".game_swf").css("padding-left", "100000px");

    if (!extraOptions) {
      extraOptions = {};
    }

    KBPAY.init(jQuery.extend({}, apiOptions_, extraOptions));

    var showOptions = {
      "iframeOnly": true,
      "width": 570,
      "height": 560,
      "onPaymentProviderClose" : onPaymentProviderClose_,
      "onPaymentWallClose" : onPaymentWallClose_
    };

    paymentWallInstance_ = KBPAY.showPaymentWall(showOptions);
  };

  var init_ = function() {
    setupServerInfo_();
    setupCssPath_();
    loadApi_();
    setupApi_();
  };

  var loadApi_ = function() {
    loadJs_("/js/jquery-1.7.1.min.js");
    loadJs_("/js/KBPAY_api.js");
  };

  var loadJs_ = function(path) {
    var newScript = document.createElement('script');

    newScript.type = 'text/javascript';
    newScript.src = [protocol_, serverInfo_["server"], path].join("");

    var firstScript = document.getElementsByTagName('script')[0];

    firstScript.parentNode.insertBefore(newScript, firstScript);
  };

  var onPaymentWallClose_ = function() {
    $(".game_swf").css("padding-left", "0px");
    $(".game_swf")[0].updatePlayerCredits();
  };

  var onPaymentProviderClose_ = function() {
    if (paymentWallInstance_ === null) {
      return;
    }

    paymentWallInstance_.close();
    paymentWallInstance_ = null;
  };

  var setupApi_ = function() {
    apiOptions_ = {
      "gameid" : serverInfo_["gameId"],
      "lang": "en",
      "paymentServer": [protocol_, serverInfo_["server"]].join(""),
      "serverid": "1",
      "css": cssPath_
    };
  };

  var setupCssPath_ = function() {
    cssPath_ = [document.location.protocol, "//", document.location.host,
        "/css/skin_rotmg.css?", g_cacheBuster].join("");
  };

  var setupKabamAccount_ = function(naid, accessToken) {
    apiOptions_["platform"] = "kabam";
    apiOptions_["userid"] = naid;
    apiOptions_["accessToken"] = accessToken;
  };

  var setupRotmgAccount_ = function(naid, signedRequest) {
    apiOptions_["platform"] = "rotmg";
    apiOptions_["userid"] = naid;
    apiOptions_["signedRequest"] = signedRequest;
  };

  var setupServerInfo_ = function() {
    if ((document.location.host == "www.realmofthemadgod.com") ||
        (document.location.host == "realmofthemadgod.appspot.com")) {
      serverInfo_ = SERVER_INFO_["production"];
      return;
    }

    if (rotmg.UrlLib.getParam("kps") == "p") {
      serverInfo_ = SERVER_INFO_["production"];
      return;
    }

    serverInfo_ = SERVER_INFO_["testing"];
  };

  init_();

  return {
    "diaplayBeginnersPackagePayment": diaplayBeginnersPackagePayment_,
    "displayPaymentWall": displayPaymentWall_,
    "setupKabamAccount": setupKabamAccount_,
    "setupRotmgAccount": setupRotmgAccount_
  };
})(jQuery);
