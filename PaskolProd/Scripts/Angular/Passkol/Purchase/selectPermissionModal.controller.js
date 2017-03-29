(function () {
    'use strict';

    angular
        .module('passkolApp')
        .controller('selectPermissionCtrl', selectPermissionCtrl);
    selectPermissionCtrl.$inject = ['$scope', '$uibModalInstance', 'purchaseService', 'appModals','$timeout','music']
    function selectPermissionCtrl($scope, $uibModalInstance, purchaseService, appModals, $timeout, music) {
        // Data
        $scope.music = music;
        $scope.strPermission = '';

        // Methods
        $scope.categorySelected = categorySelected;
        $scope.isValid = isValid;
        $scope.isCompleted = false;
        $scope.continueToBuy = continueToBuy;
        $scope.getPermissionCost = getPermissionCost;
        $scope.close = closeModal;
        // Init
        load();

        // Methods Implementation
        function closeModal() {
            $uibModalInstance.close();
        }

        function continueToBuy() {
            if ($scope.finalCost > 0) {
                getPurchaseModal();
            } else {
                createPhonePurchase();
            }
        }

        function createPhonePurchase() {
            var data = {
                valueIds: getPermissionValuesIds(),
                musicId: $scope.music.ID,
                categoryId: $scope.selectedCategory.ID
            };

            purchaseService.phonePurchase(data, $scope.antiForgeryToken)
                .then(function success(evt) {
                    if (evt.data) {
                        $scope.buyOkMode = true;
                        $scope.buyMode = true;
                        $scope.isPhonePurchase = true;
                        $timeout(closeModal, 7000);
                    } else {
                        failed();
                    }
                }, failed);
        }

        function failed(evt) {
            appModals.alertModal('פסקול','אירעה שגיאה!');
        }
        function getPurchaseModal() {
            var data = {
                valueIds: getPermissionValuesIds(),
                musicId: $scope.music.ID,
                categoryId: $scope.selectedCategory.ID,
                cost: $scope.finalCost
            };
            purchaseService.getPurchaseUrl(data, $scope.antiForgeryToken)
                .success(function (resUrl) {
                    $scope.IframeSrc = resUrl.URL;
                    $scope.buyMode = true;
                    $scope.buyOkMode = false;
                });
        }

        $scope.handlePeleCardRes = function (res) {
            if (res.success) {
                $scope.buyOkMode = true;
                $timeout(closeModal, 7000);
            } else {
                //document.getElementById('peleIFrmae').contentWindow.location.href = res.UrlRefferer;
                $scope.IframeSrc = res.UrlRefferer;
                $scope.pelecardErr = res.errMessage
            }
            $scope.$apply();
        }
            
        function isValid() {
            var valid = checkedIfCompleted() && $scope.acceptAgreement;
            return valid;
        }

        function getPermissionCost() {
            if (!checkedIfCompleted()) {
                $scope.finalCost = 0;
            } else {
                var data = {
                    valueIds: getPermissionValuesIds(),
                    musicId : $scope.music.ID,
                    categoryId : $scope.selectedCategory.ID
                };
                purchaseService.getPermissionCost(data, $scope.antiForgeryToken).then(function success(evt) {
                    if (!evt.data.Success) {
                        appModals.alertModal('פסקול', 'אירעה שגיאה בבדיקת מחיר הרשיון!');
                    } else {
                        $scope.finalCost = evt.data.Cost;
                    }
                }, function failed(evt) {
                    appModals.alertModal('פסקול','אירעה שגיאה בבדיקת מחיר הרשיון!');
                });

                $scope.strPermission = getStringPermission();
            }
        }

        function getStringPermission() {
            return $scope.selectedCategory.PermissionProperties.map(
                function(prop){
                    return prop.Name + ' - ' + prop.PropertyValueViews.find(
                        function (val) {
                            return val.ID === prop.ValueSelected;
                        }
                    ).Name;
                }
            ).join(', ');
        }

        function getPermissionValuesIds() {
            return $scope.selectedCategory.PermissionProperties.map(function(prop) { 
                return prop.ValueSelected;
            }).filter(function (id) {
                return id;
            });
        }
        function checkedIfCompleted() {
            $scope.isCompleted = !!($scope.selectedCategory && $scope.selectedCategory.PermissionProperties && 
                $scope.selectedCategory.PermissionProperties.length && 
                getPermissionValuesIds().length == $scope.selectedCategory.PermissionProperties.length);
            return $scope.isCompleted;
        }

        function load() {
            purchaseService.getPermissionCategories()
                .then(getPermissionCategoriesSuccess, getPermissionCategoriesFailed);
        }

        function getPermissionCategoriesSuccess(evt) {
            if (evt.data.Success) {
                $scope.permissionCategories = evt.data.Categories;
            }
        }

        function getPermissionCategoriesFailed() {
            appModals.alertModal('פסקול','אירעה שגיאה בהבאת נתוני הרישיון!')
        }

        function categorySelected(category) {
            if ($scope.selectedCategory !== category) {
                $scope.selectedCategory = category;
                getPermissionCost();
                $scope.isCompleted = false;
                if (!$scope.selectedCategory.PermissionProperties) {
                    $scope.finalCost = 0;
                    purchaseService.permissionCategoryDetails(category.ID)
                        .then(function success(evt) {
                            if (!evt.data.Success) {
                                getPermissionCategoriesFailed();
                            } else {
                                var resCategory = evt.data.Category;
                                category.PermissionProperties = resCategory.PermissionProperties;
                            } 
                        },
                        getPermissionCategoriesFailed);
                }
            }
            
            $scope.showCategorySelect = false;
        }
    }
})();