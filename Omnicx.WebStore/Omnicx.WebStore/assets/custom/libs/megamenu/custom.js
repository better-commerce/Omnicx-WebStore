var headerFunctions = {

    // initialise everything
    init: function () {

        // GLOBAL VARS
        _cF = this;
        _cF._mobWidth = 768,
            _cF._tabWidth = 992,
            _cF._deskWidth = 1200,
            _cF._dropDownCount = 0,
            _cF._navCount = 0,
            _cF._tapCount = 0,
            _cF._navTolerance = 100,
            _cF._megaNavTolerance = 500,
            _cF._topNavHover = false,
            _cF._megaNavHover = false,
            _cF._menuTouched,
            _cF._megaMenuTouched,
            _cF._backBtnTouched,
            _cF._overlayTouched,
            _cF._prevOption,
            _cF._winWidth = Math.max(document.documentElement.clientWidth, window.innerWidth || 0),
            _cF._dropDownMenuBtns = document.getElementsByClassName('hasDropDownMenu'),
            _cF._mobileMenuBtns = document.getElementsByClassName('mobileMenuBtn'),
            _cF._mobileNavBackBtns = document.getElementsByClassName('mobileMenuBack'),
            _cF._mobileSearchBtn = document.querySelector('.mobileSearchBtn'),
            _cF._mobileSearchContainer = document.querySelector('.mobShowHide'),
            _cF._menuSiteOverlay = document.querySelector('.navOverlay'),
            _cF._siteNav = document.querySelector('.siteNav'),
            _cF._topNav = document.querySelector('.menu'),
            _cF._topNavItems = document.getElementsByClassName('menu__item'),
            _cF._megaNavBlock = document.querySelector('.megaNav'),
            _cF._megaNavOptions = document.getElementsByClassName('menuOption'),
            _cF._azMenuItems = document.getElementsByClassName('alphabetBar__list__item'),
            _cF._azOptions = document.getElementsByClassName('alphabetOption');



        // DEBUG




        // WINDOW EVENTS
        window.addEventListener('resize', _cF.windowResize, false);



        // GLOBAL
        // SVG polyfil for IE9
        //svg4everybody();



        // FUNCTIONS
        // IE9 input placeholder polyfil
        _cF.ie9PlaceHolder();

        // mega nav
        _cF.megaMenuInit();

        // mobile search button to show form
        if (_cF._mobileSearchBtn) { _cF._mobileSearchBtn.addEventListener('click', _cF.mobileSearch, false); }
        // reset menus on site overlay click
        if (_cF._menuSiteOverlay) { _cF._menuSiteOverlay.addEventListener('click', _cF.menuResets, false); }



        // WINDOW WIDTH ON LOAD
        // on page load & if mobile width, do:
        if (_cF._winWidth < _cF._mobWidth) { // mobile

            // cycle plugin
            _cF.cycle(true);
            _cF.addClass(_cF._siteNav, 'mobTransfrom');

        };



        //LOOPS
        // if exist: mobile menu toggle class - there are 2 of these buttons
        if (_cF._mobileMenuBtns) {

            for (var i = 0, _mobileMenuBtn; _mobileMenuBtn = _cF._mobileMenuBtns[i]; i++) {
                _mobileMenuBtn.addEventListener('click', _cF.mobileMenuBtns, false);
            };

        };

        // simple drop down menus
        for (var i = 0, _dropDownMenuBtn; _dropDownMenuBtn = _cF._dropDownMenuBtns[i]; i++) {
            _cF.dropDownMenus(_dropDownMenuBtn);
        };


    },

    // window resize & resets
    windowResize: function () {

        // update window width on resize
        _cF._winWidth = Math.max(document.documentElement.clientWidth, window.innerWidth || 0);

        _cF.menuResets();
        _cF.megaMenuInit();

        if (_cF._winWidth >= _cF._mobWidth) { // tablet & desktop

            // destroy cycle plugin
            _cF.cycle(false);
            _cF.removeClass(_cF._siteNav, 'mobTransfrom');


        } else { // mobile

            // init cycle plugin
            _cF.cycle(true);
            _cF.addClass(_cF._siteNav, 'mobTransfrom');

        };

    },

    // hasClass
    hasClass: function (elem, className) {
        if (elem != undefined)
            return new RegExp(' ' + className + ' ').test(' ' + elem.className + ' ');
    },

    // addClass
    addClass: function (elem, className) {
        if (!_cF.hasClass(elem, className)) {
            if (elem != undefined)
                elem.className += ' ' + className;
        }
    },

    // removeClass
    removeClass: function (elem, className) {
        if (elem != null && elem != undefined) {

            var newClass = ' ' + elem.className.replace(/[\t\r\n]/g, ' ') + ' ';
            if (_cF.hasClass(elem, className)) {

                while (newClass.indexOf(' ' + className + ' ') >= 0) {
                    newClass = newClass.replace(' ' + className + ' ', ' ');
                }

                elem.className = newClass.replace(/^\s+|\s+$/g, '');
            }
        }
    },

    // toggleClass
    toggleClass: function (elem, className) {

        var newClass = ' ' + elem.className.replace(/[\t\r\n]/g, " ") + ' ';

        if (_cF.hasClass(elem, className)) {

            while (newClass.indexOf(" " + className + " ") >= 0) {
                newClass = newClass.replace(" " + className + " ", " ");
            }

            elem.className = newClass.replace(/^\s+|\s+$/g, '');

        } else {
            elem.className += ' ' + className;
        }
    },

    // Simple hover / touch drop down menus - use class 'hasDropDownMenu' on the element you wish to trigger the menu
    dropDownMenus: function (_this) {

        var _menuToDrop = _this.nextElementSibling, // in HTML put <ul> straight after the <a> you wish to use this function with
            _tapCount = 0,
            _touchBtn;

        if (Modernizr.touchevents) {

            _touchBtn = new Hammer(_this);

            _touchBtn.on('tap press', function (ev) {
                touchDropDowns();
            });

        } else {

            _this.addEventListener('mouseenter', hoverOnDropDownMenus, false);
            _this.addEventListener('mouseleave', hoverOffDropDownMenus, false);
            _menuToDrop.addEventListener('mouseenter', hoverOnDropDownMenus, false);
            _menuToDrop.addEventListener('mouseleave', hoverOffDropDownMenus, false);

        };

        function hoverOnDropDownMenus() {

            _cF._dropDownCount++;

            if (this == _this) {
                _cF.addClass(_menuToDrop, 'is-active');
            }

        };

        function hoverOffDropDownMenus() {

            _cF._dropDownCount--;

            setTimeout(function () {

                if (!_cF._dropDownCount) {
                    _cF.removeClass(_menuToDrop, 'is-active');
                }

            }, _cF._navTolerance);

        };

        function touchDropDowns() {

            if (_tapCount == 0) {
                _tapCount++;
                _cF.addClass(_menuToDrop, 'is-active');
            } else {
                _tapCount = 0;
                _cF.removeClass(_menuToDrop, 'is-active');
            }

        };

    },

    // Mega Menu functions - NOTE: Mobile Menu & Desktop Mega Menu are the same markup
    megaMenuInit: function () {

        if (_cF._megaNavBlock) { // if a mega nav exists

            var _menuToShow,
                _menuOptionShown;

            if (Modernizr.touchevents && _cF._winWidth >= _cF._mobWidth) { // devices & tablet / desktop width

                _cF.megaMenuTouch();

                // page load always show first option
                _cF.addClass(_cF._azMenuItems[0], 'is-active');
                _cF.addClass(_cF._azOptions[0], 'is-active');

                for (var i = 0, _azMenuItem; _azMenuItem = _cF._azMenuItems[i]; i++) {
                    _azMenuItem.addEventListener('mouseenter', _cF.megaMenuAZHoverOn, false);
                };

            } else if (!Modernizr.touchevents && _cF._winWidth >= _cF._mobWidth) { // computers & tablet / desktop width

                // add hover behaviour to
                _cF._topNav.addEventListener('mouseenter', _cF.megaMenuHoverOn, false);
                _cF._topNav.addEventListener('mouseleave', _cF.megaMenuHoverOff, false);

                // loop through all top nav links and attach hover / touch function
                for (var i = 0, _topNavItem; _topNavItem = _cF._topNavItems[i]; i++) {
                    _topNavItem.addEventListener('mouseenter', _cF.megaMenuHoverOn, false);
                    _topNavItem.addEventListener('mouseleave', _cF.megaMenuHoverOff, false);
                };
                for (var i = 0, _megaNavOption; _megaNavOption = _cF._megaNavOptions[i]; i++) {
                    _megaNavOption.addEventListener('mouseenter', _cF.megaMenuHoverOn, false);
                    _megaNavOption.addEventListener('mouseleave', _cF.megaMenuHoverOff, false);
                };
                for (var i = 0, _azMenuItem; _azMenuItem = _cF._azMenuItems[i]; i++) {
                    _azMenuItem.addEventListener('mouseenter', _cF.megaMenuAZHoverOn, false);
                };

                // page load always show first option
                _cF.addClass(_cF._azMenuItems[0], 'is-active');
                _cF.addClass(_cF._azOptions[0], 'is-active');

                // site overlay behaviour
                _cF._menuSiteOverlay.addEventListener('mouseenter', _cF.menuResets, false);

            } else if (_cF._winWidth <= _cF._mobWidth) { // mobile width

                _cF.megaMenuMobWidth();

            };

        };

    },

    megaMenuMobWidth: function () {

        _cF.megaMenuRemoveEvents();
        _cF.mobileMenu();

    },

    megaMenuHoverOn: function () {

        var _linkItem = this,
            _menuToShow = this.getAttribute('data-show-menu-option'),
            _menuOptionShown = document.querySelector('div.desktopHoverItem[data-menu-option="' + _menuToShow + '"]');


        if (!_menuOptionShown) { // if the attr 'data-show-menu-option' == null

            if (_cF._topNavHover == true && (_cF._navCount == 1 || _cF._navCount == 3)) { //  mega menu option

                //console.log('------------------ MEGA NAV ON');

                _cF._navCount = 2;
                _cF._topNavHover = false;
                _cF._megaNavHover = true;

                _cF.addClass(_cF._topNav, 'is-active');

            }

        } else if (_menuOptionShown && _cF._topNavHover == false && _cF._megaNavHover == false && _cF._navCount == 0) { // first time from site

            _cF._navCount = 1;
            _cF._topNavHover = true;

            //console.log('------------------ ITEM ON: FIRST TIME');

            _cF.addClass(_linkItem, 'is-active');
            _cF.addClass(_cF._megaNavBlock, 'is-active');
            _cF.addClass(_menuOptionShown, 'is-active');
            _cF.addClass(_cF._menuSiteOverlay, 'is-active');

        } else if (_menuOptionShown && _cF._topNavHover == false && _cF._megaNavHover == false && _cF._navCount == 1) { // back to to menu item after leaving from mega menu

            //console.log('------------------ ITEM ON: AFTER LEFT FROM MEGA MENU');

            _cF._navCount = 3;
            _cF._topNavHover = true;
            _cF._megaNavHover = false;

            _cF.addClass(_linkItem, 'is-active');
            _cF.addClass(_cF._megaNavBlock, 'is-active');
            _cF.addClass(_menuOptionShown, 'is-active');
            _cF.addClass(_cF._menuSiteOverlay, 'is-active');

        } else if (_menuOptionShown && _cF._topNavHover == false && (_cF._navCount == 1 || _cF._navCount == 2) && _cF._megaNavHover == true) { // back on after mega menu

            //console.log('------------------ ITEM ON: FROM MEGA MENU');

            _cF._navCount = 3;
            _cF._topNavHover = true;
            _cF._megaNavHover = false;

            _cF.megaMenuResetClasses();

            _cF.addClass(_linkItem, 'is-active');
            _cF.addClass(_menuOptionShown, 'is-active');

        } else if (_menuOptionShown && _cF._topNavHover == true && (_cF._navCount == 1 || _cF._navCount == 3) && _cF._megaNavHover == false) { // straight to another top nav item

            //console.log('------------------ ITEM ON: TOP NAV TO TOP NAV');

            _cF._navCount = 3;
            _cF._topNavHover = true;
            _cF._megaNavHover = false;

            _cF.megaMenuResetClasses();

            _cF.addClass(_linkItem, 'is-active');
            _cF.addClass(_menuOptionShown, 'is-active');

        };

    },

    megaMenuHoverOff: function () {

        var _linkItem = this,
            _menuToShow = this.getAttribute('data-show-menu-option'),
            _menuOptionShown = document.querySelector('div.desktopHoverItem[data-menu-option="' + _menuToShow + '"]');


        if (!_menuOptionShown && _cF._topNavHover == false && _cF._megaNavHover == false && _cF._navCount == 0) { // no mega menu option used - only menu bar area

            //console.log('------------------ MENU BAR TO HEADER WITHOUT MEGA MENU');

            _cF.removeClass(_cF._topNav, 'is-active');

        } else if (!_menuOptionShown && _cF._topNavHover == false && _cF._megaNavHover == false && _cF._navCount == 1) {

            //console.log('------------------ MENU BAR TO HEADER AFTER MEGA MENU');

            _cF._navCount = 0;
            _cF._topNavHover = false;
            _cF._megaNavHover = false;

            _cF.megaMenuResetClasses();

            _cF.removeClass(_cF._topNav, 'is-active');
            _cF.removeClass(_linkItem, 'is-active');
            _cF.removeClass(_cF._megaNavBlock, 'is-active');

        } else if (!_menuOptionShown && _cF._topNavHover == false && _cF._megaNavHover == true && _cF._navCount == 2) { // mega menu to nav bar

            //console.log('------------------ MEGA NAV TO NAV BAR');

            _cF._navCount = 1;

            setTimeout(function () {

                if (_cF._navCount == 1) {

                    //console.log('------------------ MEGA NAV TIMER RESET');

                    _cF._topNavHover = false;
                    _cF._megaNavHover = false;

                    _cF.megaMenuResetClasses();

                    _cF.removeClass(_linkItem, 'is-active');
                    _cF.removeClass(_cF._megaNavBlock, 'is-active');
                    _cF.removeClass(_cF._menuSiteOverlay, 'is-active');

                }

            }, _cF._megaNavTolerance);

        } else if (_menuOptionShown && _cF._topNavHover == true && _cF._megaNavHover == false && (_cF._navCount == 1 || _cF._navCount == 3)) { // top nav item off to site or nav

            //console.log('------------------ ITEM OFF: ONTO NAV OR SITE');

            _cF._navCount = 1;

            setTimeout(function () {

                if (_cF._navCount == 1) {

                    //console.log('------------------ TOP ITEM TIMER RESET');

                    _cF._navCount = 0;
                    _cF._topNavHover = false;
                    _cF._megaNavHover = false;

                    _cF.megaMenuResetClasses();

                    _cF.removeClass(_cF._topNav, 'is-active');
                    _cF.removeClass(_linkItem, 'is-active');
                    _cF.removeClass(_cF._megaNavBlock, 'is-active');
                    _cF.removeClass(_cF._menuSiteOverlay, 'is-active');

                }

            }, _cF._megaNavTolerance);

        };

    },

    megaMenuTouch: function () {

        for (var i = 0, _menuOption; _menuOption = _cF._topNavItems[i]; i++) {
            showMenus(_menuOption);
        };

        function showMenus(_this) {

            var _linkItem = _this,
                _menuToShow = _this.getAttribute('data-show-menu-option'),
                _menuOptionShown = document.querySelector('div.desktopHoverItem[data-menu-option="' + _menuToShow + '"]');

            _cF._megaMenuTouched = new Hammer(_linkItem, { domEvents: true });

            if (_menuToShow) {

                _cF._megaMenuTouched.on('tap', function (ev) {

                    // ev.srcEvent.stopPropagation();
                    // ev.gesture.srcEvent.preventDefault();

                    // console.log(ev.target);
                    // console.log(ev.target.href);

                    if (_cF._tapCount == 0) { // first touch of top nav - show mega menu option

                        _cF._tapCount = 1;
                        _cF._prevOption = _menuToShow;

                        _cF.megaMenuResetClasses();
                        _cF.addClass(_cF._siteNav, 'is-active');
                        _cF.addClass(_linkItem, 'is-active');
                        _cF.addClass(_cF._megaNavBlock, 'is-active');
                        _cF.addClass(_menuOptionShown, 'is-active');
                        _cF.addClass(_cF._menuSiteOverlay, 'is-active');

                    } else if (_cF._tapCount == 1 && (_menuToShow != _cF._prevOption)) { // tap another top nav - show mega menu

                        _cF._prevOption = _menuToShow;

                        _cF.megaMenuResetClasses();
                        _cF.addClass(_cF._siteNav, 'is-active');
                        _cF.addClass(_linkItem, 'is-active');
                        _cF.addClass(_cF._megaNavBlock, 'is-active');
                        _cF.addClass(_menuOptionShown, 'is-active');

                    } else if (_menuToShow == _cF._prevOption) { // otherwise if 2nd tap of same top nav - load top nav page

                        _cF._tapCount = 0;

                        window.location.href = ev.target.href;

                    };

                });

            };

        };

    },

    megaMenuResetClasses: function () {

        // loop through all other menu & mega nav items and remove active classes
        for (var i = 0, _topNavItem; _topNavItem = _cF._topNavItems[i]; i++) {
            _cF.removeClass(_topNavItem, 'is-active');
        };

        for (var i = 0, _megaNavOption; _megaNavOption = _cF._megaNavOptions[i]; i++) {
            _cF.removeClass(_megaNavOption, 'is-active');
        };

    },

    megaMenuRemoveEvents: function () {

        if (Modernizr.touchevents && _cF._winWidth >= _cF._mobWidth) { // devices & tablet / desktop width


        } else if (!Modernizr.touchevents && _cF._winWidth >= _cF._mobWidth) { // computer tablet / desktop width

            _cF._menuTouched.off('tap press');
            _cF._backBtnTouched.off('tap press');

        } else if (_cF._winWidth <= _cF._mobWidth) { // mobile width

            _cF._topNav.removeEventListener('mouseenter', _cF.megaMenuHoverOn, false);
            _cF._topNav.removeEventListener('mouseleave', _cF.megaMenuHoverOff, false);

            // loop through all top nav links and attach hover / touch function
            for (var i = 0, _topNavItem; _topNavItem = _cF._topNavItems[i]; i++) {
                _topNavItem.removeEventListener('mouseenter', _cF.megaMenuHoverOn, false);
                _topNavItem.removeEventListener('mouseleave', _cF.megaMenuHoverOff, false);
            };
            for (var i = 0, _megaNavOption; _megaNavOption = _cF._megaNavOptions[i]; i++) {
                _megaNavOption.removeEventListener('mouseenter', _cF.megaMenuHoverOn, false);
                _megaNavOption.removeEventListener('mouseleave', _cF.megaMenuHoverOff, false);
            };

            _cF._menuSiteOverlay.removeEventListener('mouseenter', _cF.menuResets, false);

        }

    },

    megaMenuAZHoverOn: function () {

        var _linkItem = this,
            _menuToShow = this.getAttribute('data-show-menu-option'),
            _menuOptionShown = document.querySelector('div.desktopHoverItem[data-menu-option="' + _menuToShow + '"]');

        for (var i = 0, _azMenuItem; _azMenuItem = _cF._azMenuItems[i]; i++) {
            _cF.removeClass(_azMenuItem, 'is-active');
        };

        for (var i = 0, _azOption; _azOption = _cF._azOptions[i]; i++) {
            _cF.removeClass(_azOption, 'is-active');
        };

        _cF.addClass(_linkItem, 'is-active');
        _cF.addClass(_menuOptionShown, 'is-active');

    },

    mobileMenu: function () {

        for (var i = 0, _menuOption; _menuOption = _cF._topNavItems[i]; i++) {
            showMenus(_menuOption);
        };

        for (var i = 0, _mobileNavBackBtn; _mobileNavBackBtn = _cF._mobileNavBackBtns[i]; i++) {
            hideMenus(_mobileNavBackBtn);
        };

        function showMenus(_this) {

            var _menuToShow = _this.getAttribute('data-show-menu-option'),
                _menuOptionShown = document.querySelector('div.desktopHoverItem[data-menu-option="' + _menuToShow + '"]');

            _cF._menuTouched = new Hammer(_this);

            if (_menuToShow) {

                _cF._menuTouched.on('tap', function (ev) {

                    _cF.addClass(_cF._megaNavBlock, 'is-active');
                    _cF.addClass(_menuOptionShown, 'is-active');

                });

            }

        };

        function hideMenus(_this) {

            _cF._backBtnTouched = new Hammer(_this);

            _cF._backBtnTouched.on('tap', function (ev) {

                _cF.removeClass(_cF._megaNavBlock, 'is-active');

                for (var i = 0, _megaNavOption; _megaNavOption = _cF._megaNavOptions[i]; i++) {
                    _cF.removeClass(_megaNavOption, 'is-active');
                };

            });

        };

    },

    mobileSearch: function (e) {

        e.preventDefault();

        // mobile search show / hide
        _cF.toggleClass(this, 'is-active');
        _cF.toggleClass(_cF._mobileSearchContainer, 'active-mobSearch');

    },

    // Mobile menu
    mobileMenuBtns: function () {

        _cF.toggleClass(_cF._mobileMenuBtns[0], 'is-active');
        _cF.toggleClass(_cF._mobileMenuBtns[1], 'is-active');
        _cF.toggleClass(_cF._siteNav, 'is-active');
        _cF.removeClass(_cF._megaNavBlock, 'is-active');
        _cF.toggleClass(_cF._menuSiteOverlay, 'is-active');

        for (var i = 0, _megaNavOption; _megaNavOption = _cF._megaNavOptions[i]; i++) {
            _cF.removeClass(_megaNavOption, 'is-active');
        };

    },

    // breakpoint resets
    menuResets: function () {

        if (_cF._winWidth < _cF._mobWidth) { // mobile

            // remove classes from mobile specific elements
            _cF.removeClass(_cF._mobileSearchContainer, 'active-mobSearch');

            for (var i = 0, _mobileMenuBtn; _mobileMenuBtn = _cF._mobileMenuBtns[i]; i++) {
                _cF.removeClass(_mobileMenuBtn, 'is-active');
            };

        } else { // tablet & desktop

            // reset A-Z mega menu options
            for (var i = 0, _azMenuItem; _azMenuItem = _cF._azMenuItems[i]; i++) {
                _cF.removeClass(_azMenuItem, 'is-active');
            };
            for (var i = 0, _azOption; _azOption = _cF._azOptions[i]; i++) {
                _cF.removeClass(_azOption, 'is-active');
            };

            _cF.addClass(_cF._azMenuItems[0], 'is-active');
            _cF.addClass(_cF._azOptions[0], 'is-active');

        };

        // shared elements
        _cF.removeClass(_cF._siteNav, 'is-active');
        _cF.removeClass(_cF._topNav, 'is-active');
        _cF.removeClass(_cF._megaNavBlock, 'is-active');

        for (var i = 0, _topNavItem; _topNavItem = _cF._topNavItems[i]; i++) {
            _cF.removeClass(_topNavItem, 'is-active');
        };

        for (var i = 0, _megaNavOption; _megaNavOption = _cF._megaNavOptions[i]; i++) {
            _cF.removeClass(_megaNavOption, 'is-active');
        };

        // site overlay
        _cF.removeClass(_cF._menuSiteOverlay, 'is-active');

        // tablet / desktop touch count
        _cF._tapCount = 0;

    },

    cycle: function (_boolean) {

        if (_boolean) { // true

            // cycle
            $('.promos__container').cycle({ slides: '> a', swipe: true, fx: 'scrollHorz', log: false });

        } else { // false

            // cycle plugin - if exists then destroy - prevents any further action on window.resize event
            if ($('.promos__container').data('cycle.opts')) {

                $('.promos__container').cycle('destroy');
            };

        };

    },

    ie9PlaceHolder: function () {

        if (!Modernizr.csstransforms3d) {

            // IE9 input placeholder polyfil
            $('.siteSearch input[type="text"]').placeholder({ customClass: 'siteSearch__placeholder' });
            $('.mobBrandSearch__form input[type="text"]').placeholder({ customClass: 'mobBrandSearch__form__placeholder' });

        };

    }

};

// initialise
headerFunctions.init();