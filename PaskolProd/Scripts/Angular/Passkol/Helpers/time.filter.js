(function () {
    'use strict';

    angular
        .module('passkolApp')
        .filter('time', time);
    time.$inject = ['$filter'];
    function time($filter) {
        return function (seconds) {
            var roundSeconds = Math.round(seconds);
            var minutes = Math.floor(roundSeconds / 60);
            var remindSeconds = roundSeconds - (minutes * 60);
            var hours = Math.floor(minutes / 60);
            var remindMinutes = minutes - (hours * 60);
            var minutesStr = remindMinutes + ':';
            var hoursStr = '';
            if (hours > 0) {
                hoursStr = hours + ':';
                minutesStr = $filter('padNumber')(remindMinutes,2) + ':';
            }
            return hoursStr + minutesStr + $filter('padNumber')(remindSeconds, 2);
        };
    }
})();