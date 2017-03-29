angular.module("passkolApp")
    .filter('range', function () {
        return function (input, min, max) {
            min = parseInt(min); //Make string input int
            max = parseInt(max);
            for (var i = min; i <= max; i++)
                input.push(i);
            return input;
        };
    })
    .filter('EmbedUrl', function ($sce) {
         return function (url) {
             return $sce.trustAsResourceUrl(url);
        };
    })
    .filter('durationview', ['datetimeSrv', function (datetimeSrv) {
        return function (input, text) {
            if (!isNaN(input)) {
                if (input > 0) {
                    var duration = datetimeSrv.duration(input);
                    return '<p class="timerXL">S</P><p class="timerXXL">' + duration.seconds + '</p><p class="timerXL">: M</p><p class="timerXXL">' + duration.minutes + '</p><p class="timerXL">: H</P><p class="timerXXL">' + duration.hours + '</p> <p class="timerXL">: D</P><p class="timerXXL">' + duration.days + '</p>';
                } else {
                    return text;
                }
            }
    };
}]);