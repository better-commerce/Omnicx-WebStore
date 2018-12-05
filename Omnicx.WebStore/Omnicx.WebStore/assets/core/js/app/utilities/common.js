//Validation Json
function isJSON(str) {
    try {
        return (JSON.parse(str) && !!str);
    } catch (e) {
        return false;
    }
}
//Validation use only for digit
function isNumberValidate(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

function validatePostCode(postCode, countryCode) {
    var country = regxGlobalPostCodes.find(x => x.ISO === countryCode);
    if (country != undefined && country != null) {
        var regex = new RegExp(country.Regex);
        return regex.test(postCode);
    }
    return false;
}