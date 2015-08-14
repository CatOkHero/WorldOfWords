app.controller('WordsController', function ($scope, $window, $timeout, $modal, $modalInstance, ModalService, WordsService, LanguageService, ConstService, language) {
    $scope.initialize = function () {
        $scope.selectedLanguageId = 0;
        LanguageService.getAllLanguages()
        .then(function (response) {
            $scope.languages = response;
        });
        $scope.valueOfWordToAdd = '';
        $scope.descriptionOfWordToAdd = '';
        $scope.transcriptionOfWordToAdd = '';
        $scope.languageIdOfWordToAdd = language;
        $scope.typeOfAdding = ($scope.languageIdOfWordToAdd == 4) ? "translation" : "word";
    }

    $scope.selectLanguage = function()
    {
        WordsService.getWordsByLanguageId($scope.selectedLanguageId)
        .then(function (response) {
            $scope.words = response;
        });
    }

    $scope.addWord = function () {
        if($scope.valueOfWordToAdd === '' || $scope.languageOfWordToAdd===null)
        {
            ModalService.showResultModal(ConstService.failureTitleForModal, ConstService.messageWhenSomeRequiredFieldsAreEmpty, false);
            return;
        }
        var newWord = {
            value: $scope.valueOfWordToAdd,
            description: $scope.descriptionOfWordToAdd,
            transcription: $scope.transcriptionOfWordToAdd,
            languageId: $scope.languageIdOfWordToAdd
        }

        WordsService.addWord(newWord)
        .then(function (newWordId) {
            if (newWordId != 0) {
                ModalService.showResultModal(ConstService.successTitleForModal, ConstService.messageWhenWordIsAdded, true);
            }
            else {
                ModalService.showResultModal(ConstService.failureTitleForModal, ConstService.messageWhenWordIsntAdded, false);
            }
        });
        $scope.close();
    }

    $scope.close = function () {
        $modalInstance.close();
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.initialize();
});
