
angular.module("passkolApp")
    .directive('phoneVld', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elem, attr, ctrl) {
                // add a parser that will process each time the value is
                // parsed into the model when the user updates it.
                ctrl.$parsers.unshift(function (value) {
                    // test and set the validity after update.
                    var isValid = false;

                    if (value === undefined || value === "") {
                        isValid = true;
                    } else {
                        var regexp = /^\d+$/;
                        if (regexp.test(value) && (value.length >= 6)) {
                            isValid = true;
                        }
                    }

                    ctrl.$setValidity('phoneVld', isValid);

                    // if it's valid, return the value to the model, 
                    // otherwise return undefined.
                    return isValid ? value : undefined;
                });

                // add a formatter that will process each time the value 
                // is updated on the DOM element.
                ctrl.$formatters.unshift(function (value) {
                    // validate.
                    // test and set the validity after update.
                    var isValid = false;

                    if (value === undefined || value === "") {
                        isValid = true;
                    } else {
                        var regexp = /^\d+$/;
                        if (regexp.test(value) && (value.length >= 6)) {
                            isValid = true;
                        }
                    }

                    ctrl.$setValidity('phoneVld', isValid);
                    // return the value or nothing will be written to the DOM.
                    return value;
                });
            }
        };
    })
    .directive('passwordVld', function () {
        return {
            // restrict to an attribute type.
            restrict: 'A',

            // element must have ng-model attribute.
            require: 'ngModel',

            // scope = the parent scope
            // elem = the element the directive is on
            // attr = a dictionary of attributes on the element
            // ctrl = the controller for ngModel.
            link: function (scope, elem, attr, ctrl) {

                // add a parser that will process each time the value is 
                // parsed into the model when the user updates it.
                ctrl.$parsers.unshift(function (value) {
                    // test and set the validity after update.
                    var isValid = false;

                    // Validate letters
                    if (/[a-z]/.test(value)) {
                        if (/[0-9]/.test(value)) {
                            if (value.length >= 6) {
                                isValid = true;
                            }
                        }
                    }
                    ctrl.$setValidity('passwordVld', isValid);

                    // if it's valid, return the value to the model, 
                    // otherwise return undefined.
                    return isValid ? value : undefined;
                });

                // add a formatter that will process each time the value 
                // is updated on the DOM element.
                ctrl.$formatters.unshift(function (value) {
                    // validate.
                    // test and set the validity after update.
                    var isValid = false;

                    // Validate letters
                    if (/[a-z]/.test(value)) {
                        if (/[0-9]/.test(value)) {
                            if (value.length >= 6) {
                                isValid = true;
                            }
                        }
                    }
                    ctrl.$setValidity('passwordVld', isValid);
                    // return the value or nothing will be written to the DOM.
                    return value;
                });
            }
        };
    })
    .directive('autoComplete', function () {
        return {
            require: 'ngModel',
            restrict: 'A',
            link: function (scope, elem, attr, ctrl) {
                elem.autocomplete({
                    source: function (req, res) {
                        $.ajax({
                            method: 'POST',
                            url: attr.url + req.term,
                            success: function (data) {
                                res(data);
                            }
                        });
                    },
                    select: function (event, ui) {
                        scope.$apply();
                    }
                });
            }
        };
    })
    .directive('contenteditable', ['$sce', function ($sce) {
        return {
            restrict: 'A', // only activate on element attribute
            require: '?ngModel', // get a hold of NgModelController
            link: function (scope, element, attrs, ngModel) {
                if (!ngModel) return; // do nothing if no ng-model

                // Specify how UI should be updated
                ngModel.$render = function () {
                    element.html($sce.getTrustedHtml(ngModel.$viewValue || ''));
                };

                element.addClass('withoutOutline');

                // Listen for change events to enable binding
                element.on('blur keyup change', function () {
                    scope.$evalAsync(read);
                });
                read(); // initialize

                // Write data to the model
                function read() {
                    var html = element.html();
                    // When we clear the content editable the browser leaves a <br> behind
                    // If strip-br attribute is provided then we strip this out
                    if (attrs.stripBr && html == '<br>') {
                        html = '';
                    }
                    ngModel.$setViewValue(html);
                }
            }
        };
    }])
    .directive('progressBarUploader', ["$timeout", function ($timeout) {
        return {
            template: '<div class="progress" hidden="hidden">' +
                            '<div class="progress-bar" role="progressbar" aria-valuemin="0" aria-valuemax="100" style="width:0%;">' +
                            '</div>' +
                        '</div><div ng-transclude></div>',
            scope: {
                url: '=',
                parent: '='
            },
            transclude: true,
            restrict: 'E',
            link: function (scope, elem, attr, ctrl) {
                scope.internalScope = scope.parent || {};
                scope.internalScope.uploadFile = function () {
                    $('.progress').removeAttr("hidden");

                    var formData = new FormData();
                    var file = elem.find('input[type=file]')[0];
                    var inputs = elem.children().find('input').not('[type=file]');
                    for (var i = 0; i < inputs.length; i++) {
                        formData.append(inputs[i].name, inputs[i].value);
                    }
                    formData.append(file.name, file.files[0]);
                    ajax = new XMLHttpRequest();
                    ajax.upload.addEventListener("progress", progressHandler, false);
                    ajax.addEventListener("load", completeHandler, false);
                    ajax.open("POST", scope.url);
                    ajax.send(formData);
                }

                function getNestedInput() {
                    nestedElements = elem.children();
                    for (var i = 0; i < nestedElements; i++) {
                        console.log(nestedElements[i])
                    }
                }
                function progressHandler(event) {
                    var percent = (event.loaded / event.total) * 100;
                    $('.progress-bar').css({ 'width': percent + '%' }); //from bootstrap bar class
                }

                function completeHandler() {
                    $('.progress-bar').css({ 'width': '100%' });
                    $timeout(function () {
                        window.location.reload();
                    }, 500);

                }
            }
        };
    }]);
    //.directive('bottomFooter', function () {
    //    var directive = {
    //        restrict: 'C',
    //        link: link
    //    };
    //    return directive;

    //    function link() {
    //        var footer = $('[class="container-fluid container-footer"]');
    //        $(window).resize(fixFooter);
    //        $(window).on('load', fixFooter);
            
    //        function fixFooter() {
    //            if (footer.length) {
    //                footer.css('margin-top', '0px');
    //                if (window.innerHeight > $('body').height()) {
    //                    footer.css('margin-top', (window.innerHeight - $('body').height() + 1) + 'px');
    //                }
    //            }
    //        }
    //    }
    //});