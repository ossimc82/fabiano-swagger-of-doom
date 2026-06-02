var rotmg = rotmg || {};

rotmg.UrlLib = (function() {
  var GET_PARAM_KEY_PREFIX = "k";

  var params_ = null;

  var getProtocolString_ = function() {
    var flashVars = [],
        location = document.location,
        protocol = location.protocol,
        port = location.port;

    flashVars.push("rotmg_loader_protocol=" + protocol);

    if (((protocol == "https:") && (port != "443")) ||
        ((protocol == "http:") && (port != "80"))) {
      flashVars.push("rotmg_loader_port=:" + port);
    }

    return flashVars.join("&");
  };

  var getParam_ = function(key) {
    parseQueryString_();

    key = GET_PARAM_KEY_PREFIX + key;

    if (params_[key] == undefined) {
      return "";
    }

    return params_[key];
  };

  var parseQueryString_ = function() {
    if (params_ != null) {
      return;
    }

    params_ = {};

    var queryString = location.search.substring(1),
        re = /([^&=]+)=([^&]*)/g,
        matches;

    while (matches = re.exec(queryString)) {
      params_[GET_PARAM_KEY_PREFIX + decodeURIComponent(matches[1])] =
          decodeURIComponent(matches[2]);
    }
  };

  return {
    getParam: getParam_,
    getProtocolString: getProtocolString_
  };
})();
