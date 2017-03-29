(function () {
    'use strict';

    angular
        .module('passkolApp')
        .directive('selectPermission', SelectPermissionDirective);
    SelectPermissionDirective.$inject = []
    function SelectPermissionDirective() {
        var directive = {
            restrict: 'E',
            link: link
        };

        return directive;

        function link(scope, element, attrs) {
            // Data
            var rightPanel = element.find('#selectPermissionRightPanel');
            var leftPanel = element.find('#selectPermissionLeftPanel');
            var selectPermsissionFrm = element.find('#selectPermsissionFrm');
            var acceptBtnContainer = element.find('#acceptBtnContainer');

            // Events
            scope.$watch(isRightPanelGrater, fixLeftPanelHeight);

            // Methods

            // Methods Implenetation
            function isRightPanelGrater() {
                var rightPanelHeight = rightPanel.height();
                var leftPanelHeight = leftPanel.height();
                var frmHeight = selectPermsissionFrm.height();
                return (rightPanelHeight > leftPanelHeight) && frmHeight - leftPanelHeight;
            }

            function fixLeftPanelHeight(newValue, oldValue) {
                console.log(scope);
                if (newValue) {
                    if (newValue > 0) {
                        acceptBtnContainer.css('margin-top', newValue);
                    } else  {
                        acceptBtnContainer.css('margin-top', 0);
                    }
                }
            }
        }
    }
})();