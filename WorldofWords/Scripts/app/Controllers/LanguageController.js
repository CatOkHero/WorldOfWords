app.controller('LanguageController', LanguageCtrl);

function LanguageCtrl($scope, $modal, ModalService, LanguageService) {
    $scope.addLanguage = function () {
        if ($scope.newLanguageName) {
            var newLanguage = {
                name: $scope.newLanguageName
            };

            LanguageService.addLanguage(newLanguage)
                .then(
                function (newLanguageId) {
                    if (newLanguageId > 0) {
                        ModalService.showResultModal('Add language', 'Language added successfully', true);
                        init();
                    } else {
                        ModalService.showResultModal('Add language', 'Failed to add language', false);
                    }
                },
                function (error) {
                    ModalService.showResultModal('Add language', 'Failed to add language', false);
                });
        }
    }

    $scope.removeLanguage = function (index) {
        var modalInstance = $modal.open({
            templateUrl: 'confirmModal',
            controller: 'ConfirmModalController',
            size: 'sm',
            backdrop: 'static',
            keyboard: false,
            resolve: {
                titleText: function () {
                    return 'Delete Language';
                },
                bodyText: function () {
                    return 'Are you sure you want to delete ' + $scope.languages[index].Name +
                           '? This action will delete all ' + $scope.languages[index].Name.toLowerCase() + ' words and Word Suites.';
                }
            }
        });

        modalInstance.result.then(function (answer) {
            if (answer) {
                LanguageService
                    .removeLanguage($scope.languages[index].Id)
                        .then(
                            function (success) {
                                ModalService.showResultModal('Delete language', 'Language deleted successfully', true);
                                init();
                            },
                            function (error) {
                                ModalService.showResultModal('Delete language', 'Failed to delete language. It may be assigned to some courses.', false);
                            }
                        );
                }
        });
    }

    var init = function () {
        LanguageService.getAllLanguages()
            .then(
            function (languages) {
                for (var i = 0; i < languages.length; i++) {
                    if (languages[i].Name === 'Ukrainian') {
                        languages.splice(i, 1);
                        break;
                    }
                }
                $scope.languages = languages;
                $scope.newLanguageName = null;
            },
            function (error) {
                ModalService.showResultModal('Load languages', 'Failed to load languages', false);
            });
    };

    init();
}