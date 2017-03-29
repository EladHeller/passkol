(function () {
    'use strict';

    angular
        .module('passkolApp')
        .filter('padNumber', PadNumberFilter);

    function PadNumberFilter() {
        return function (number, pad) {
            return (1e4 + number + "").slice(-pad);
        };
    }
})();