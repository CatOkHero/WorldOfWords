app.controller('GlobalDictionaryController', function ($scope, $window, $timeout, $modal, WordTranslationService, ConstService, LanguageService) {
    $scope.initialize = function () {
        $scope.selectedPage = 1;
        $scope.searchValue = '';
        $scope.isLanugageSelected = false;
        $scope.WORDS_ON_PAGE = ConstService.WORDS_ON_PAGE;
        $scope.selectedLanguageId = 0;
        $scope.wordsAmount = 0;
        LanguageService.getAllLanguages()
        .then(function (response) {
            $scope.languages = response;
            for(var i = 0; i<$scope.languages.length; ++i)
            {
                if($scope.languages[i].Name === "Ukrainian")
                {
                    $scope.languages.splice(i, 1);
                    break;
                }
            }
        });
    }

    $scope.changeDataToShow = function () {
        if ($scope.searchValue === '') {
            WordTranslationService.getAmountOfWordTranslationsBySpecificLanguage($scope.selectedLanguageId)
            .then(function (response) {
                $scope.wordsAmount = response;
        WordTranslationService.getWordTranslationsFromInterval(($scope.selectedPage - 1) * ConstService.WORDS_ON_PAGE, Math.min($scope.selectedPage * ConstService.WORDS_ON_PAGE, $scope.wordsAmount), $scope.selectedLanguageId)
        .then(function (response) {
            $scope.toShow = response;
        });
            });
        } else {
            WordTranslationService.getAmountOfWordTranslationBySearchValue($scope.searchValue, $scope.selectedLanguageId)
        .then(function (response) {
            $scope.wordsAmount = response;
                WordTranslationService.getWordTranslationBySearchValueFromInterval(($scope.selectedPage - 1) * ConstService.WORDS_ON_PAGE, Math.min($scope.selectedPage * ConstService.WORDS_ON_PAGE, $scope.wordsAmount), $scope.selectedLanguageId, $scope.searchValue)
            .then(function (response) {
                 $scope.toShow = response;
            });
        });
    }
    }

    $scope.selectLanguage = function () {
        $scope.isLanguageSelected = true;
        $scope.selectedPage = 1;
        $scope.searchValue = '';
        $scope.changeDataToShow();
    }

    $scope.open = function () {
        var modalInstance = $modal.open({
            animation: true,
            templateUrl: 'Views/CreateWordTranslation.html',
            controller: 'CreateWordTranslationController',
            size: 'lg',
            resolve: {
                languageId: function () {
                    return $scope.selectedLanguageId;
                }
            }
        });
    }

    $scope.openImportModal = function () {
        var modalInstance = $modal.open({
            templateUrl: 'modalImport',
            controller: 'ImportWordTranslationsController',
            size: 'lg',
            resolve: {
                languageId: function () {
                    return $scope.selectedLanguageId;
                }
            }
        });

        modalInstance.result.then(function (importedWordTranslations) {
            if (importedWordTranslations) {
                $scope.importedWordTranslations = importedWordTranslations;
                $scope.openImportResultModal('Import Word Translations', 'Word Translations successfully imported', true, 'Create Word Suite from Word Translations');
            } else {
                $scope.openImportResultModal('Import Word Translations', 'Failed to import Word Translations', false, null);
            }
            $scope.changeDataToShow();
        });
    }

    $scope.openImportResultModal = function (title, body, success, actionButtonText) {
        var modalInstance = $modal.open({
            templateUrl: 'messageWithActionModal',
            controller: 'MessageWithActionModalController',
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
                },
                actionButtonText: function () {
                    return actionButtonText;
                }
            }
        });

        modalInstance.result.then(function (action) {
            if (success) {
                if (action) {
                    $scope.createWordSuite();
                }
            }
        });
    }

    $scope.createWordSuite = function () {
        var wordSuiteData = {
            importedWordTranslations: $scope.importedWordTranslations,
            languageId: Number($scope.selectedLanguageId)
        }
        localStorage.setItem('wordSuiteData', JSON.stringify(wordSuiteData));
        $window.location.href = 'Index#/CreateWordSuite';
    }

    $scope.initialize();
});
