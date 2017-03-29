(function() {
    'use strict';

    angular
        .module('passkolApp')
        .directive('fileInput', FileInputDirective);

    FileInputDirective.$inject = ['appModals'];
    
    function FileInputDirective(appModals) {
        var directive = {
            link: link,
            restrict: 'E',
            templateUrl: 'fileInput.template',
            scope: {
                file: '=',
                type: '=',
                resetFile: '='
            }
        };
        return directive;

        function link(scope, element, attrs) {
            var fileInput = element.find('input');
            fileInput.bind('change', function (evt) {
                scope.$apply(function () {
                    if (evt.target.files[0]) {
                        if (evt.target.files[0].type === scope.type) {
                            scope.file = evt.target.files[0];
                        } else {
                            appModals.alertModal('העלאת קובץ','ניתן להעלות קובץ מסוג ' + scope.type + ' בלבד!');
                        }
                    }
                });
            });

            scope.resetFile = function () {
                fileInput.replaceWith(fileInput.clone(true));
                scope.file = undefined;
            }
        }
    }

})();