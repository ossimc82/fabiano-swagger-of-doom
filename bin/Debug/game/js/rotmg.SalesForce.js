function openSalesForceFirstTime(data){
	var salesForceTemplate = '<div id="rotmg_support_container" style="display:none"><iframe src="//kabam.secure.force.com/?data=DATAGOESHERE" id="iframe_salesforce" class="rotmg_support"></iframe><div id="close_iframe_salesforce" onclick="$(\'#rotmg_support_container\').fadeOut(\'2\')"  class="rotmg_support"></div></div>';
	var salesForce = salesForceTemplate.replace('DATAGOESHERE', data);

	$('body').append(salesForce);
	$('#rotmg_support_container').fadeToggle('2');
}

function reopenSalesForce(){
	$('#rotmg_support_container').fadeToggle('2');
}