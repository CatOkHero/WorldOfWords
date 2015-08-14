app.controller('UnassignedModalController',
    function ($scope,
        $modalInstance) {
        $scope.actionResult = function () {
            $modalInstance.close();
        };
    });
