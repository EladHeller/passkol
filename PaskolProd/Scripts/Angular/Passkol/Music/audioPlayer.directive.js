(function () {
    'use strict';

    angular
        .module('passkolApp')
        .directive('audioPlayer', audioPlayerDirective);

    audioPlayerDirective.$inject = [];
    
    function audioPlayerDirective() {
        var directive = {
            link: link,
            restrict: 'E',
            templateUrl: 'audioPlayer.template'
        };
        return directive;

        function link(scope, element, attrs) {
            // Data
            scope.audioPlayer = scope.audioPlayer || new Audio();
            scope.progresWidth = { width: '5px' };
            var mscSources = [];
            var currSource = 0;
            var currProgressWidth = 0;
            var progressBar = $('.player-content-progress');

            // Functions
            scope.changeAudioSource = changeAudioSource;
            scope.playClicked = playClicked;
            scope.timelineClicked = timelineClicked;

            // Events
            scope.audioPlayer.addEventListener('error', onAudioError);
            scope.audioPlayer.addEventListener('loadeddata', onDataLoaded);
            scope.audioPlayer.addEventListener('timeupdate', audioPlayer_onTimeUpdate);
            scope.audioPlayer.addEventListener('ended', audioPlayer_onEnded);
            
            // Init
            $(".player-content-progress-cursor").draggable({
                axis: 'x',
                containment: ".player-content-progress",
                stop: moveCursorEnd
            });


            // Method implement
            function audioPlayer_onEnded(evt) {
                scope.audioPlayer.currentTime = 0;
            }

            function getProgressBarWidth() {
                return progressBar.width();
            }
            function getProgressBarOffsetX() {
                return progressBar.offset().left;
            }

            function playClicked() {
                if (scope.audioPlayer.paused) {
                    scope.audioPlayer.play();
                } else {
                    scope.audioPlayer.pause();
                }
            }

            function changeAudioSource(sources) {
                if (Array.isArray(sources)) {
                    mscSources = sources;
                } else {
                    mscSources = [sources];
                }

                if (scope.audioPlayer.src !== mscSources[0]) {
                    currSource = 0;
                    currProgressWidth = 5;
                    scope.audioLoaded = false;
                    scope.audioError = false;
                    scope.audioPlayer.src = mscSources[0];
                } else {
                    scope.audioPlayer.currentTime = 0;
                    scope.audioPlayer.play();
                }
            }

            function onDataLoaded(evt) {
                scope.audioLoaded = true;
                scope.$apply();
                scope.audioPlayer.play();
            }

            function onAudioError(evt) {
                if ((currSource + 1) < mscSources.length) {
                    scope.audioPlayer.src = mscSources[++currSource];
                } else {
                    scope.audioError = true;
                }
            }

            function moveCursorEnd(event, ui) {
                moveCursor(event.clientX - getProgressBarOffsetX());
            }

            function timelineClicked($event) {
                moveCursor($event.clientX - getProgressBarOffsetX());
            }

            function audioPlayer_onTimeUpdate(evt) {
                currProgressWidth = (scope.audioPlayer.currentTime / scope.audioPlayer.duration) * getProgressBarWidth();
                scope.progresWidth = { width: (currProgressWidth + 5) + 'px' };
                scope.$apply();
            }

            function moveCursor(positionLeft) {
                if (scope.audioPlayer.seekable.end(0) > 0) {
                    currProgressWidth = positionLeft;
                    scope.audioPlayer.currentTime = scope.audioPlayer.duration * positionLeft / getProgressBarWidth();
                }
                $(".player-content-progress-cursor").css('left', '0px');
                scope.progresWidth = { width: (currProgressWidth + 5) + 'px' };
            }
        }
    }
})();