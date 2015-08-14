app.controller('EditWordSuiteController', EditWordSuiteCtrl);

function EditWordSuiteCtrl($scope, $window, $modal, $routeParams, ModalService, UserService, WordSuiteService, WordTranslationService, LanguageService) {
    $scope.newWordTranslations = [];

    $scope.searchWordTranslations = function (searchWord) {
        return WordTranslationService
            .searchForWordTranslations(searchWord, $scope.wordSuite.LanguageId)
                .then(
                function (wordTranslations) {
                    return wordTranslations;
                },
                function (error) {
                    ModalService.showResultModal('Search Word Translations', 'Error while searching Word Translations', false);
                });
    }

    $scope.removeExistingWordTranslation = function (index) {
        wordTranslationsToDeleteIdRange.push($scope.existingWordTranslations[index].Id);
        $scope.existingWordTranslations.splice(index, 1);
    }

    $scope.removeNewWordTranslation = function (index) {
        $scope.newWordTranslations.splice(index, 1);
    }

    $scope.addWordTranslationToWordSuite = function () {
        if ($scope.selectedWordTranslation && $scope.selectedWordTranslation.Id) {
            for (var i = 0; i < $scope.existingWordTranslations.length; i++) {
                if ($scope.selectedWordTranslation.Id === $scope.existingWordTranslations[i].Id) {
                    ModalService.showResultModal('Add Word Translation', 'This Translation already exists in Word Suite', false);
                    return;
                }
            }

            for (var i = 0; i < $scope.newWordTranslations.length; i++) {
                if ($scope.selectedWordTranslation.Id === $scope.newWordTranslations[i].Id) {
                    ModalService.showResultModal('Add Word Translation', 'You have already added this Translation', false);
                    return;
                }
            }

            $scope.newWordTranslations.push($scope.selectedWordTranslation);
            $scope.selectedWordTranslation = null;
        }
    }

    $scope.openImportModal = function () {
        var modalInstance = $modal.open({
            templateUrl: 'modalImport',
            controller: 'ImportWordTranslationsController',
            size: 'lg',
            resolve: {
                languageId: function () {
                    return $scope.wordSuite.LanguageId;
                }
            }
        });

        modalInstance.result.then(function (importedWordTranslations) {
            for (var i = 0; i < importedWordTranslations.length; i++) {
                var wordFound = false;

                for (var j = 0; j < $scope.newWordTranslations.length; j++) {
                    if (importedWordTranslations[i].Id === $scope.newWordTranslations[j].Id) {
                        wordFound = true;
                        break;
                    }
                }

                if (!wordFound) {
                    for (var j = 0; j < $scope.existingWordTranslations.length; j++) {
                        if (importedWordTranslations[i].Id === $scope.existingWordTranslations[j].Id) {
                            wordFound = true;
                            break;
                        }
                    }
                }

                if (!wordFound) {
                    $scope.newWordTranslations.push(importedWordTranslations[i]);
                }
            }
        });
    }

    $scope.saveWordSuite = function () {
        if (checkInput() && ($scope.newWordTranslations.length || $scope.existingWordTranslations.length)) {
            for (var i = 0; i < $scope.newWordTranslations.length; i++) {
                wordTranslationsToAddIdRange.push($scope.newWordTranslations[i].Id);
            }

            if (checkIfAreAnyChanges()) {
                WordSuiteService
                    .editWordSuite({
                        Id: $scope.wordSuite.Id,
                        Name: $scope.wordSuite.Name,
                        LanguageId: $scope.wordSuite.LanguageId,
                        Threshold: $scope.wordSuite.Threshold,
                        QuizResponseTime: $scope.wordSuite.QuizResponseTime,
                        WordTranslationsToAddIdRange: wordTranslationsToAddIdRange,
                        WordTranslationsToDeleteIdRange: wordTranslationsToDeleteIdRange,
                        IsBasicInfoChanged: isBasicInfoChanged
                    })
                    .then(
                    function (ok) {
                        showResultModal('Edit Word Suite', 'Word Suite successfully edited', true);
                    },
                    function (badRequest) {
                        showResultModal('Edit Word Suite', badRequest.Message, false);
                    });
            }
        }
    }

    $scope.createWordTranslation = function () {
        var modalInstance = $modal.open({
            animation: true,
            templateUrl: 'Views/CreateWordTranslation.html',
            controller: 'CreateWordTranslationController',
            size: 'lg',
            backdrop: 'static',
            keyboard: false,
            resolve: {
                languageId: function () {
                    return $scope.wordSuite.LanguageId;
                }
            }
        });
    }

    var oldName = null;
    var oldThreshold = null;
    var oldQuizResponseTime = null;
    var isBasicInfoChanged = false;
    var wordTranslationsToAddIdRange = [];
    var wordTranslationsToDeleteIdRange = [];

    var checkInput = function () {
        $scope.isEnteredName = Boolean($scope.wordSuite.Name);
        $scope.isEnteredThreshold = Boolean($scope.wordSuite.Threshold);
        $scope.isEnteredQuizResponseTime = Boolean($scope.wordSuite.QuizResponseTime);

        return $scope.isEnteredName && $scope.isEnteredThreshold && $scope.isEnteredQuizResponseTime;
    }

    var checkIfAreAnyChanges = function () {
        isBasicInfoChanged = oldName !== $scope.wordSuite.Name ||
                             oldThreshold !== $scope.wordSuite.Threshold ||
                             oldQuizResponseTime != $scope.wordSuite.QuizResponseTime;

        return isBasicInfoChanged || wordTranslationsToAddIdRange.length || wordTranslationsToDeleteIdRange.length;
    }

    var showResultModal = function (title, body, success) {
        var modalInstance = $modal.open({
            templateUrl: 'messageModal',
            controller: 'MessageModalController',
            size: 'sm',
            backdrop: 'static',
            keyboard: false,
            resolve: {
                titleText: function () {
                    return title;
                },
                bodyText: function () {
                    return body;
                },
                success: function () {
                    return success;
                }
            }
        });

        modalInstance.result.then(function () {
            if (success) {
                $window.location.href = 'Index#/WordSuites';
            }
        });
    }

    var openWordSuite = function (id) {
        WordSuiteService
            .getWordSuiteByID(id)
                .then(function (wordSuite) {
                    $scope.wordSuite = wordSuite;
                    if ($scope.wordSuite) {
                        oldName = $scope.wordSuite.Name;
                        oldThreshold = $scope.wordSuite.Threshold;
                        oldQuizResponseTime = $scope.wordSuite.QuizResponseTime;

                        WordTranslationService
                        .getWordTranslationsByWordSuiteID($scope.wordSuite.Id)
                            .then(function (wordTranslations) {
                                $scope.existingWordTranslations = wordTranslations;
                            })
                    }
                });
    }

    openWordSuite($routeParams.wordSuiteId);
}

