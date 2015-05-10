
		var ProviderController = new function()
		{
			var _providerList;
			
			this.selectProvider = function(e)
			{
				var currentSelectedProvider = _providerList.getSelectedProvider();
				var index = currentSelectedProvider.getPayoutSet().getSelectedIndex();
				
				var providerId = e.currentTarget.getAttribute("name");
				// select the new provider
				var providerToSelect = _providerList.getProviderById(providerId);
				providerToSelect.setSelected(true);
				
				var payoutSetToSelect = providerToSelect.getPayoutSet();
				var newIndex = Math.min(index, payoutSetToSelect.getCount()-1);
				payoutSetToSelect.selectPayoutByIndex(newIndex);
				
				if(providerToSelect.getType() === "redeem")
				{
					var redeemInput = document.getElementById("redeemInput");
					if(redeemInput)
					{
						redeemInput.focus();
					}
					$(".footerContainer .buttonContainer").hide();
					$("#step2").css("visibility", "hidden");
				}
				else
				{
					$(".footerContainer .buttonContainer").show();
					$("#step2").css("visibility", "visible");
				}
			};
	
			this.init = function(providerList)
			{
				_providerList = providerList;
			};
			
		}();
			
		
		var PayoutController = new function()
		{
			var _providerList;
			var _tooltip;
			var _customData;
	
			var _getPayoutId = function(e)
			{
				var name = e.currentTarget.getAttribute("name");
				if(e.currentTarget.tagName.toLowerCase() === "li" && name)
				{
					return name;
				}
				else
				{
					return $(e.currentTarget).parents("li")[0].getAttribute("name");
				}
			};
			
			this.selectPayout = function(e)
			{
				var payoutId = _getPayoutId(e);
				var payoutSet = _providerList.getSelectedProvider().getPayoutSet();
				var payout = payoutSet.getPayoutById(payoutId);
				payout.setSelected(true);
			};

			var _paymentClose = function(data)
			{
				var message = {
					"method"	: "providerClose"
				};
										
				parent.postMessage(JSON.stringify(message), "*");
			};
			
			this.buySelectedPayout = function(e)
			{
				var payoutSet = _providerList.getSelectedProvider().getPayoutSet();
				var selectedPayout = payoutSet.getSelectedPayout();
				var isLocalCurrency = _providerList.getSelectedProvider().getName() === KBPAY.Provider.FACEBOOK_LOCAL_CURRENCY;
				var extraParams = {};
				if(isLocalCurrency)
				{
					extraParams = 
					{
						"quantity": 1,		// treating payout as a package
						"isLocalCurrency" : isLocalCurrency,
						"custom_data": _customData
					};
				}
				KBPAY.buy(selectedPayout.getId(), _paymentClose, extraParams);
			};

			this.showOffer = function(e)
			{
				var isLocalCurrency =  _providerList.getSelectedProvider().getName() === KBPAY.Provider.FACEBOOK_LOCAL_CURRENCY;
				var providerId = e.currentTarget.getAttribute("name");
				// select the new provider
				var offerProvider = _providerList.getProviderById(providerId);
				
				var offerId = offerProvider.getId();
			
				var isLocalCurrency =  _providerList.getSelectedProvider().getName() === KBPAY.Provider.FACEBOOK_LOCAL_CURRENCY;
				KBPAY.showOffer({"offerid": offerId, "isLocalCurrency": isLocalCurrency}, _paymentClose);
			};

			this.redeem = function(e)
			{
				// make sure pin number is valid
				var pin = document.getElementById("pin").value;
				if($.trim(pin) == "")
				{
					var dialog = new KBPAY.Dialog('<div style="width:250px;padding:20px">'+KBPAY.i18n.translate('Please enter a pin/promo code.')+'</div>');
					dialog.show();
					return;
				}

				// show loading
				var redeemLoading = document.getElementById("redeemLoading");
				redeemLoading.style.display = "block";
				KBPAY.redeem(pin, 
					function(response)
					{
						redeemLoading.style.display = "none";
						if(!response || response.error)
						{
							if(! response || response.error == "Unrecognized pin.")
							{
								var dialog = new KBPAY.Dialog('<div style="width:250px;padding:20px">'+KBPAY.i18n.translate('Pin code not recognized')+'</div>');
									dialog.show();
							}
							else
							{
								var dialog = new KBPAY.Dialog('<div style="width:250px;padding:20px">' + response.error + '</div>');
								dialog.show();
							}
						}
						else
						{
							var dialog = new KBPAY.Dialog('<div style="width:250px;padding:20px">'+KBPAY.i18n.translate('Successfully Redeemed!')+'</div>');
							dialog.show();
						}
					}
				);
			};

			this.close = function(e)
			{
				var message = {
						"method"	: "paymentIframeClose",
						"iframeDialogId" : iframeDialogId
					};
											
					parent.postMessage(JSON.stringify(message), "*");
			};
			
			this.showCatalogItems = function(e)
			{
				// determine the catalog items
				var payoutId = _getPayoutId(e);
				var payoutSet = _providerList.getSelectedProvider().getPayoutSet();
				var payout = payoutSet.getPayoutById(payoutId);
				var catalogItems = payout.getProperty("catalogItems");
				if(payout.getNumOfBonusItems() <= 0 || !catalogItems)
				{
					return;
				}
				
				// clear catalog items tooltip
				var tooltipContent = $(_tooltip).children(".content")[0];
				tooltipContent.innerHTML = "";
				
				// set Tooltip Title
				var catalogTitle = payout.getProperty("catalogTitle") ? "<span>" + pay.lang.en(payout.getProperty("catalogTitle")) + "</span>" : "";
				$(_tooltip).children(".top").html(catalogTitle);
				
				var i, catalogItem;
				for(i = 0; i < catalogItems.length; i++)
				{
					catalogItem = catalogItems[i];
					tooltipContent.innerHTML += '<div class="catalogItem">' + catalogItem.title + " x " + catalogItem.numOfItems + '</div>';
				}
					
				
				_tooltip.style.visibility = "hidden";
				_tooltip.style.display = "block";
	
				// determine x
				var target = $(e.currentTarget);
				var targetLeft = target.offset().left;
				var targetWidth =target.width();
				var tooltipWidth = $(_tooltip).outerWidth();
				var normalX = targetLeft + targetWidth;
				var flippedX = Math.max(targetLeft - tooltipWidth, 0);
				var x = document.body.clientWidth > normalX + tooltipWidth ? normalX : flippedX;
				
				//determine y
				var targetTop = target.offset().top;
				var targetHeight = target.height();
				var tooltipHeight = $(_tooltip).height();
				var normalY = targetTop;
				var flippedY = Math.max(targetTop - tooltipHeight, 0);
				var y = document.body.clientHeight > normalY + tooltipHeight ? normalY : flippedY;
				
				_tooltip.style.left = x + "px";
				_tooltip.style.top = y + "px";
				_tooltip.style.visibility = "visible";
				
			};
			
			this.hideCatalogItems = function(e)
			{
				_tooltip.style.display = "none";
			};
			
			this.init = function(providerList, tooltipContainer, customData)
			{
				_providerList = providerList;
				_tooltip = tooltipContainer;
				_customData = customData;
			};
			
		}();

		var PageController = new function()
		{
			var _renderTemplate = function(providerInfos, templateURL, customData)
			{
				if(!providerInfos || typeof(providerInfos) != "object")
				{
					return
				}
				try
				{
					var paymentContainer  = document.getElementById("payments");
					paymentContainer.innerHTML = "";
			
					// get the template, process data and render UI
					$.ajax(
					{
						type: "GET",
						url: templateURL,
						dataType: "xml",
						success: function(response) 
						{
		
							// template for the main container
							var templateText = $(response).find("#mainContainer").text();
		
							// template for provider
							var providerTemplateText = $.trim($(response).find("#provider").text());
		
							// template for payout
							var payoutTemplateText = $.trim($(response).find("#payout").text());
							
							paymentContainer.innerHTML = templateText.process(providerInfos);
			
							var providerList = new KBPAY.ProviderList();
							var providerListView = new KBPAY.ProviderListView("providerList");
							providerListView.render(document.getElementById("providerContainer"));
			
							PayoutController.init(providerList, document.getElementById("tooltip"), customData);
							ProviderController.init(providerList);
		
							var gameCurrency = providerInfos.game.currencyName;
							
							// process payoutsets
							for(j= 0; j < providerInfos.payoutSets.length; j++)
							{
								providerInfo = providerInfos.payoutSets[j];
								providerInfo.type = "payment";
			
								// Alias allow us to rename a payment provider 
								switch(providerInfo.providerName.toLowerCase())
								{
									case "boku":
										providerInfo.alias = KBPAY.i18n.translate("Mobile");
										break;
									case "facebooklocalcurrency":
										providerInfo.alias = "Facebook";
										break;
									default:
										providerInfo.alias = providerInfo.providerName;
								}
								// unique id allow one payment provider to apprear more than once in the view
								providerInfo.uniqueId = providerInfo.providerId + "_" + j;
			
								// create payoutset and provider list model
								payoutInfos = providerInfo.payouts;
			
								// show provider only if it has payout
								if(payoutInfos.length > 0)
								{
									provider = new KBPAY.Provider(providerInfo);
									payoutSet = new KBPAY.PayoutSet();
									provider.setPayoutSet(payoutSet);
									providerList.add(provider);
									
									// create provider view
									providerListItemView = new KBPAY.ProviderListItemView(provider, providerTemplateText);
									providerListView.add(providerListItemView);
									payoutSetView = new KBPAY.PayoutSetView(provider, "packages");
									payoutSetView.render(document.getElementById("packageContainer"));
									
									// handling payment provider click (make it selected)
									$(providerListItemView.getHtmlElement()).bind("click", ProviderController.selectProvider);
									
			
									// process payouts
									for(i = 0; i < payoutInfos.length; i++)
									{
										payoutInfo = payoutInfos[i];
										if(payoutInfo.show)
										{
											// create the payout model in the front-end
											payout = new KBPAY.Payout(payoutInfo);
											payout.setIgcName(gameCurrency);
											payoutSet.add(payout);
			
											// create payout view
											payoutView = new KBPAY.PayoutView(payout, gameCurrency, payoutTemplateText, null, providerInfos.game.settings);
											
											// hanle payout view click (make it selected)
											$(payoutView.getHtmlElement()).bind("click", PayoutController.selectPayout);
			
											if(payout.getNumOfBonusItems() > 0)
											{
												// handling mouse enter / leave the bonus items area
												$(payoutView.getFirstChildByClass(".bonusItems")).bind("mouseover", PayoutController.showCatalogItems);
												$(payoutView.getFirstChildByClass(".bonusItems")).bind("mouseleave", PayoutController.hideCatalogItems);
											}
											payoutSetView.add(payoutView);
										}
									}
								}
							}
		
							if(providerInfos.offer)
							{
								var offerProvider = new KBPAY.Provider(
									{
										"uniqueId"		: providerInfos.offer.id + "_" + j,
										"providerId"	: providerInfos.offer.id,
										"providerName"	: "Offer",
										"alias"			: "Earn " + gameCurrency,
										"type"			: "offer"
									}
								);
								providerList.add(offerProvider);
								var offerProviderListItemView = new KBPAY.ProviderListItemView(offerProvider, providerTemplateText);
								$(offerProviderListItemView.getHtmlElement()).bind("click", PayoutController.showOffer);
								providerListView.add(offerProviderListItemView);
							}
		
							// handling buy button click
							$("#buyButton").bind("click", PayoutController.buySelectedPayout);
							$("#redeemButton").bind("click", PayoutController.redeem);
							$("#closeButton").bind("click", PayoutController.close);
						}
					});
				}
				catch(e)
				{
					if(console && console.log)
					{
						console.log(e);
					}
					document.getElementById("payments").innerHTML = "error rendering template";
				}
		
			};

			this.onWindowLoad = function(options, payoutList, templateURL)
			{
				try
				{
					if(!hasError)
					{
						KBPAY.init(options);
						if(typeof(jsTranslation) === "object")
						{
							KBPAY.i18n.init(options.lang, jsTranslation);
						}
						var custom_data = options.custom_data ? options.custom_data : null;
						_renderTemplate(payoutList, templateURL, custom_data);
					}
					else
					{
						document.getElementById("payments").innerHTML = 
						'<div style="text-align:center">\
							<span style="display:inline-block;background:#FFF;color:#000;border:1px solid #000; margin-top:30px;min-width:350px">\
								<div class="titleBar"></div>\
								<div style="margin:20px;">\
									<h4>${errorMessage}</h4>\
									<input type="button" class="iframeDialogCloseButton" value="OK" />\
								</div>\
							</span>\
						</div>'.process();
					}
				}
				catch(e)
				{
					// error
				}
			};
		}();
		
					
