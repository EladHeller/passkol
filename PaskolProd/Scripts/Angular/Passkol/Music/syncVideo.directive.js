(function() {
    'use strict';

    angular
        .module('passkolApp')
        .directive('syncVideo', syncVideo);

    syncVideo.$inject = [];
    
    function syncVideo () {
        var directive = {
            link: link,
            restrict: 'E',
            templateUrl: 'syncVideo.template'
        };
        return directive;

        function link(scope, element, attrs) {
            var frd = new FileReader();
            var fileInput = element.find('.upload-file-input');
            var fileNameElement = element.find('.upload-file-text');
            var videoElement = element.find('#syncVideoPlayer')[0];
            videoElement.muted = true;

            fileInput.on('change', function (evt) {
                var file = this.files[0];
                if (file) {
                    fileNameElement.text(file.name);
                    frd.readAsDataURL(file);
                }
            });
            frd.addEventListener('loadend', function (evt) {
                scope.videoLoad = true;
                videoElement.src = this.result;
            });
        }
    }
})();