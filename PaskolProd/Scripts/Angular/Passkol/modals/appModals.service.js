(function () {
    'use strict';

    angular
        .module('passkolApp')
        .factory('appModals', appModals);

    appModals.$inject = ['$uibModal'];

    function appModals($uibModal) {
        var service = {
            shareMusicModal: shareMusicModal,
            shareMusicMailModal:shareMusicMailModal,
            alertModal:alertModal,
            selectPermissionModal: selectPermissionModal,
            editMusicModal: editMusicModal
        };

        return service;

        function editMusicModal(music, antiForgeryToken,tags, search) {
            return $uibModal.open({
                templateUrl: 'editMusicModal.template',
                controller: 'editMusicModalCtrl',
                size: 'lg',
                resolve: {
                    music: function () {
                        return music;
                    },
                    antiForgeryToken: function () {
                        return antiForgeryToken;
                    },
                    tags: function () {
                        return tags;
                    },
                    search: function () {
                        return search;
                    }
                }
            });
        }

        function shareMusicModal(music, antiForgeryToken) {
            return $uibModal.open({
                templateUrl: 'shareMusic.template',
                controller: 'shareMusicCtrl',
                size: 'md',
                resolve: {
                    music: function () {
                        return music;
                    },
                    antiForgeryToken: function () {
                        return antiForgeryToken;
                    }
                }
            });
        }

        function shareMusicMailModal(music, antiForgeryToken) {
            return $uibModal.open({
                templateUrl: 'shareMusicMail.template',
                controller: 'shareMusicMailCtrl',
                size: 'md',
                resolve: {
                    music: function () {
                        return music;
                    },
                    antiForgeryToken: function () {
                        return antiForgeryToken;
                    }
                }
            });
        }

        function alertModal(subject, text) {
            return $uibModal.open({
                templateUrl: 'alertModal.template',
                controller: 'alertModalCtrl',
                resolve: {
                    text: function () {
                        return text;
                    },
                    subject: function () {
                        return subject;
                    }
                }
            });
        }

        function selectPermissionModal(music) {
            return $uibModal.open({
                templateUrl: 'selectPermissionModal.template',
                controller: 'selectPermissionCtrl',
                size: 'md',
                resolve: {
                    music: function () {
                        return music;
                    }
                }
            });
        }

        function purchaseModal(purchaseData) {
            return $uibModal.open({
                templateUrl: 'purchase.template',
                controller: 'selectPermissionCtrl',
                size: 'md',
                resolve: {
                    purchaseData: function () {
                        return purchaseData;
                    }
                }
            });
        }
    }
})();