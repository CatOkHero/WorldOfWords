app.controller('ConfirmModalController', function ($scope, $modalInstance, titleText, bodyText) {
    $scope.titleText = titleText;
    $scope.bodyText = bodyText;

    $scope.yes = function () {
        $modalInstance.close(true);
    }

    $scope.no = function () {
        $modalInstance.close(false);
    }
});