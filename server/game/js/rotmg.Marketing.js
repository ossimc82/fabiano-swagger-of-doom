var rotmg = rotmg || {};

rotmg.Marketing = (function() {
  var pixelsMap_ = {
    "landing": "https://kabam1-a.akamaihd.net/pixelkabam/html/pixels/rmgdirectp2.html",
    "install": "https://kabam1-a.akamaihd.net/pixelkabam/html/pixels/rmgp1.html",
    "tutorialComplete": "https://kabam1-a.akamaihd.net/pixelkabam/html/pixels/rmgp2.html"
  };

  var appendIframe_ = function(url) {
    var pixelFrame = document.createElement("IFRAME"); 

    pixelFrame.setAttribute("src", url);
    pixelFrame.setAttribute("frameborder", "0"); 
    pixelFrame.style.width = "1px"; 
    pixelFrame.style.height = "1px";
    pixelFrame.style.overflow = "hidden";
    pixelFrame.style.border = "none";
    document.body.appendChild(pixelFrame); 
  };

  var track_ = function(name) {
    if (pixelsMap_[name] == undefined) {
      return;
    }

    if ((document.location.host != "www.realmofthemadgod.com") &&
        (document.location.host != "realmofthemadgod.appspot.com")) {
      return;
    }

    appendIframe_(pixelsMap_[name]);
  };

  return {
    track: track_
  };
})();
