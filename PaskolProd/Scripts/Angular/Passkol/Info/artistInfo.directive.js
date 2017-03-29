(function() {
    'use strict';

    angular
        .module('passkolApp')
        .directive('infoPage', infoPage);

    infoPage.$inject = ['appModals','appConfig'];
    
    function infoPage(appModals, appConfig) {
        var directive = {
            link: link,
        };
        return directive;

        function link(scope, element, attrs) {

            scope.uploadContent = function () {
                if (appConfig.userConfig.isAuthenticated &&
                    appConfig.userConfig.userType &&
                    appConfig.userConfig.userType !== scope.userTypes.Artist) {
                    appModals.alertModal('פסקול', 'העלאת תוכן פתוחה ליוצרים בלבד!');
                } else {
                    location.href = '/Artist/MySongs?upload'
                }
            }
        }
    }

})();