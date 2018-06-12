(function () {
    'use strict';
    window.app.factory('scriptLoader', function () {
        return {
            load: function (url, type, rel) {
                if (type === undefined) type = 'text/javascript';
                if (url) {
                    var script = document.querySelector("script[src*='" + url + "']");
                    if (!script) {
                        var heads = document.getElementsByTagName("head");
                        if (heads && heads.length) {
                            var head = heads[0];
                            if (head) {
                                if (!rel) {
                                    script = document.createElement('script');
                                    script.setAttribute('src', url)
                                } else {
                                    script = document.createElement('link');
                                    script.setAttribute('href', url)
                                }
                                script.setAttribute('type', type);
                                if (rel) script.setAttribute('rel', rel);
                                head.appendChild(script);
                            }
                        }
                    }
                    return script;
                }
            }
        };
    });
}());