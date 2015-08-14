app.controller('ImportWordTranslationsController', function ($scope, $modal, $modalInstance, WordTranslationService, languageId) {
    $scope.wordTranslations = [];
    $scope.itemsPerPage = 21;
    $scope.currentPage = 1;

    $scope.getFileName = function () {
        $scope.fileName = document.getElementById('file').value.replace('C:\\fakepath\\', '');
        $scope.$apply();
    }

    $scope.openFile = function () {
        if ($scope.fileName.indexOf('.txt') == $scope.fileName.length - 4) {
            $scope.wordTranslations = [];
            $scope.areAllSelected = true;

            var file = document.getElementById('file').files[0],
                reader = new FileReader();

            reader.onloadend = function (e) {
                var exp = /(.+)=([^\[\]\{\}]+)(?:\[(.*)\])? ?(?:\{(.*)\})?/,
                    data = (e.target.result)
                    .replace(/\t/g, '')
                    .replace(/ +/g, ' ')
                    .split('\n');

                for (var i = 0; i < data.length; i++) {
                    if (exp.test(data[i])) {
                        $scope.wordTranslations.push({
                            originalWord: exp.exec(data[i])[1].trim(),
                            translationWord: exp.exec(data[i])[2].trim(),
                            transcription: exp.exec(data[i])[3],
                            description: exp.exec(data[i])[4],
                            isChosen: true
                        });
                    }
                }

                $scope.showWordTranslations();
                $scope.$apply();
            }

            reader.readAsText(file, "utf-8");
        }
    }

    $scope.showWordTranslations = function () {
        var begin = (($scope.currentPage - 1) * $scope.itemsPerPage),
            end = begin + $scope.itemsPerPage;
        $scope.wordTranslationsToShow = $scope.wordTranslations.slice(begin, end);
    };

    $scope.selectAll = function (areAllSelected) {
        for (var i = 0; i < $scope.wordTranslations.length; i++) {
            $scope.wordTranslations[i].isChosen = areAllSelected;
        }
    }

    $scope.checkAllSelected = function () {
        for (var i = 0; i < $scope.wordTranslations.length; i++) {
            if (!$scope.wordTranslations[i].isChosen) {
                $scope.areAllSelected = false;
                return;
            }
        }
        $scope.areAllSelected = true;
    }

    $scope.import = function () {
        var wordTranslations = [];

        for (var i = 0; i < $scope.wordTranslations.length; i++) {
            if ($scope.wordTranslations[i].isChosen) {
                wordTranslations.push({
                    OriginalWord: $scope.wordTranslations[i].originalWord,
                    TranslationWord: $scope.wordTranslations[i].translationWord,
                    Transcription: $scope.wordTranslations[i].transcription,
                    Description: $scope.wordTranslations[i].description,
                    LanguageId: languageId
                });
            }
        }

        if (wordTranslations.length) {
            $scope.isProcessing = true;
            WordTranslationService
                .importWordTranslations(wordTranslations)
                .then(function (importedWordTranslations) {
                    if (importedWordTranslations) {
                        $modalInstance.close(importedWordTranslations);
                    } else {
                        $scope.openImportResultModal('Import Word Translations', 'Failed to import Word Translations', false);
                    }
                })
        }
    }

    $scope.openImportResultModal = function (title, body, success) {
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
    }

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
});