/*
Password Strength Indicator using jQuery and XML

By: Bryian Tan (bryian.tan at ysatech.com)
01122011 - v01.01.00

Description:
Password Strength Indicator somewhat similar to ASP.NET AJAX PasswordStrength extender control behavior 
and implemented by using jQuery and XML. The password information is stored in an XML file. 
Sample XML file contents:
<PasswordPolicy>
<Password>
<duration>180</duration> //password age, expired in xxx days
<minLength>14</minLength> //password minimum length
<maxLength>25</maxLength> //password maximum length
<numsLength>2</numsLength> //number of required digits  
<upperLength>1</upperLength> //number of required upper case  
<specialLength>1</specialLength> //number of required special characters 
<barWidth>200</barWidth> //the bar indicator width
<barColor>Green</barColor> //the bar indicator colors
<specialChars>!@#\$%*()_+^&amp;}{:;?.</specialChars> //allowable special characters
</Password>
</PasswordPolicy>

Resources:
http://fyneworks.blogspot.com/2007/04/dynamic-regular-expressions-in.html
http://projects.sharkmediallc.com/pass/
http://docs.jquery.com/Plugins/Authoring
http://stackoverflow.com/questions/1034306/public-functions-from-within-a-jquery-plugin
*/


(function($) {

    var password_Strength = new function() {

        //return count that match the regular expression
        this.countRegExp = function(passwordVal, regx) {
            var match = passwordVal.match(regx);
            return match ? match.length : 0;
        }

        this.getStrengthInfo = function(passwordVal) {
            var len = passwordVal.length;
            var pStrength = 0; //password strength
            var msg = "", inValidChars = ""; //message

            //get special characters from xml file
            var allowableSpecilaChars = new RegExp("[" + password_settings.specialChars + "]", "g")

            var nums = this.countRegExp(passwordVal, /\d/g), //numbers
			lowers = this.countRegExp(passwordVal, /[a-z]/g),
			uppers = this.countRegExp(passwordVal, /[A-Z]/g), //upper case
			specials = this.countRegExp(passwordVal, allowableSpecilaChars), //special characters
			spaces = this.countRegExp(passwordVal, /\s/g);

            //check for invalid characters
            inValidChars = passwordVal.replace(/[a-z]/gi, "") + inValidChars.replace(/\d/g, "");
            inValidChars = inValidChars.replace(/\d/g, "");
            inValidChars = inValidChars.replace(allowableSpecilaChars, "");

            //check space
            if (spaces > 0) {
                return "No spaces!";
            }

            //invalid characters
            if (inValidChars !== '') {
                return "Invalid character: " + inValidChars;
            }

            //max length
            if (len > password_settings.maxLength) {
                return "Password too long!";
            }

            //GET NUMBER OF CHARACTERS left
            if ((specials + uppers + nums + lowers) < password_settings.minLength) {
                msg += password_settings.minLength - (specials + uppers + nums + lowers) + " more characters, ";
            }

            //at the "at least" at the front
            if (specials == 0 || uppers == 0 || nums == 0 || lowers == 0) {
                msg += "At least ";
            }

            //GET NUMBERS
            if (nums >= password_settings.numberLength) {
                nums = password_settings.numberLength;
            }
            else {
                msg += (password_settings.numberLength - nums) + " more numbers, ";
            }

            //special characters
            if (specials >= password_settings.specialLength) {
                specials = password_settings.specialLength
            }
            else {
                msg += (password_settings.specialLength - specials) + " more symbol, ";
            }

            //upper case letter
            if (uppers >= password_settings.upperLength) {
                uppers = password_settings.upperLength
            }
            else {
                msg += (password_settings.upperLength - uppers) + " Upper case characters, ";
            }

            //strength for length
            if ((len - (uppers + specials + nums)) >= (password_settings.minLength - password_settings.numberLength - password_settings.specialLength - password_settings.upperLength)) {
                pStrength += (password_settings.minLength - password_settings.numberLength - password_settings.specialLength - password_settings.upperLength);
            }
            else {
                pStrength += (len - (uppers + specials + nums));
            }

            //password strength
            pStrength += uppers + specials + nums;

            //detect missing lower case character
            if (lowers === 0) {
                if (pStrength > 1) {
                    pStrength -= 1; //Reduce 1
                }
                msg += "1 lower case character, ";
            }

            //strong password
            if (pStrength == password_settings.minLength && lowers > 0) {
                msg = "Strong password!";
            }

            return msg + ';' + pStrength;
        }
    }

    //default setting
    var password_settings = {
        minLength: 6,
        maxLength: 15,
        specialLength: 1,
        upperLength: 1,
        numberLength: 1,
        barWidth: 200,
        barColor: 'Red',
        specialChars: '!@#$', //allowable special characters
        metRequirement: false,
        useMultipleColors: 0
    };

    //password strength plugin 
    $.fn.password_strength = function(options) {

        //check if password met requirement
        this.metReq = function() {
            return password_settings.metRequirement;
        }

        //read password setting from xml file
        $.ajax({
            type: "GET",
            url: "../Account/GetPasswordPolicy", //use absolute link if possible
            dataType: "xml",
            success: function(xml) {

                $(xml).find('Password').each(function() {
                    var _minLength = $(this).find('minLength').text(),
                    _maxLength = $(this).find('maxLength').text(),
                    _numsLength = $(this).find('numsLength').text(),
                    _upperLength = $(this).find('upperLength').text(),
                    _specialLength = $(this).find('specialLength').text(),
                    _barWidth = $(this).find('barWidth').text(),
                    _barColor = $(this).find('barColor').text(),
                    _specialChars = $(this).find('specialChars').text(),
                    _useMultipleColors = $(this).find('useMultipleColors').text();

                    //set variables
                    password_settings.minLength = parseInt(_minLength);
                    password_settings.maxLength = parseInt(_maxLength);
                    password_settings.specialLength = parseInt(_specialLength);
                    password_settings.upperLength = parseInt(_upperLength);
                    password_settings.numberLength = parseInt(_numsLength);
                    password_settings.barWidth = parseInt(_barWidth);
                    password_settings.barColor = _barColor;
                    password_settings.specialChars = _specialChars;
                    password_settings.useMultipleColors = _useMultipleColors;
                });
            }
        });
        return this.each(function() {

            //bar position
            var barLeftPos = $("[id$='" + this.id + "']").position().left + $("[id$='" + this.id + "']").width();
            var barTopPos = $("[id$='" + this.id + "']").position().top + $("[id$='" + this.id + "']").height();

            //password indicator text container
            var container = $('<span></span>')
            .css({ position: 'absolute', top: barTopPos - 6, left: barLeftPos + 15, 'font-size': '75%', display: 'inline-block', width: password_settings.barWidth + 40 });

            //add the container next to textbox
            $(this).after(container);

            //bar border and indicator div
            var passIndi = $('<div id="PasswordStrengthBorder"></div><div id="PasswordStrengthBar" class="BarIndicator"></div>')
            .css({ position: 'absolute', display: 'none' })
            .eq(0).css({ height: 3, top: barTopPos - 16, left: barLeftPos + 15, 'border-style': 'solid', 'border-width': 1, padding: 2 }).end()
            .eq(1).css({ height: 5, top: barTopPos - 14, left: barLeftPos + 17 }).end()

            //set max length of textbox
            //$("[id$='" + this.id + "']").attr('maxLength', password_settings.maxLength);

            //add the boder and div
            container.before(passIndi);

            $(this).keyup(function() {

                var passwordVal = $(this).val(); //get textbox value

                //set met requirement to false
                password_settings.metRequirement = false;

                if (passwordVal.length > 0) {

                    var msgNstrength = password_Strength.getStrengthInfo(passwordVal);

                    var msgNstrength_array = msgNstrength.split(";"), strengthPercent = 0,
                    barWidth = password_settings.barWidth, backColor = password_settings.barColor;

                    //calculate the bar indicator length
                    if (msgNstrength_array.length > 1) {
                        strengthPercent = (msgNstrength_array[1] / password_settings.minLength) * barWidth;
                    }

                    //$("[id$='PasswordStrengthBorder']").css({ display: 'inline', width: barWidth });

                    //use multiple colors
                    //if (password_settings.useMultipleColors === "1") {
                    //    //first 33% is red
                    //    if (parseInt(strengthPercent) >= 0 && parseInt(strengthPercent) <= (barWidth * .33)) {
                    //        backColor = "red";
                    //    }
                    //    //33% to 66% is blue
                    //    else if (parseInt(strengthPercent) >= (barWidth * .33) && parseInt(strengthPercent) <= (barWidth * .67)) {
                    //        backColor = "blue";
                    //    }
                    //    else {
                    //        backColor = password_settings.barColor;
                    //    }
                    //}

                    //$("[id$='PasswordStrengthBar']").css({ display: 'inline', width: strengthPercent, 'background-color': backColor });

                    //remove last "," character
                    if (msgNstrength_array[0].lastIndexOf(",") !== -1) {
                        container.text(msgNstrength_array[0].substring(0, msgNstrength_array[0].length - 2));
                    }
                    else {
                        container.text(msgNstrength_array[0]);
                    }

                    if (strengthPercent == barWidth) {
                        password_settings.metRequirement = true;

                    }

                }
                else {
                    container.text('');
                    $("[id$='PasswordStrengthBorder']").css("display", "none"); //hide
                    $("[id$='PasswordStrengthBar']").css("display", "none"); //hide
                }
            });
        });
    };

})(jQuery);
