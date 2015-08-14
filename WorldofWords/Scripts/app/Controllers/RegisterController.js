app.controller('RegisterController',
    function ($scope,
        $window,
        $modal,
        $modalInstance,
        RegisterService,
        ConstService) {
        var onceClicked = ConstService.zero;
        $scope.strengthValue = false;
        $scope.customStyle = {};
        $scope.closeModal = function () {
            $modalInstance.close();
        };

        $scope.turnYellow = function () {
            $scope.customStyle.style = {'color': 'orange'};
        }
        $scope.turnGreen = function () {
            $scope.customStyle.style = { 'color': 'green' };
        }
        $scope.turnRed = function () {
            $scope.customStyle.style = { 'color': 'red' };
        }

        $scope.registerButtonClick = function () {
            if (!onceClicked) {
                onceClicked++;
                $scope.errorMessage = '';

                var value = {
                    login: $scope.nameValue + ' ' + $scope.surnameValue,
                    password: $scope.passwordValue,
                    email: $scope.emailValue,
                };
                if (!$scope.nameValue) {
                    $scope.errorMessage = ConstService.invalidName;
                    onceClicked = ConstService.zero;
                } else if (!$scope.surnameValue) {
                    $scope.errorMessage = ConstService.invalidName;
                    onceClicked = ConstService.zero;
                } else if (!$scope.emailValue) {
                    $scope.errorMessage = ConstService.invalidEmail;
                    onceClicked = ConstService.zero;
                } else if (!$scope.passwordValue || !$scope.repeatPasswordValue) {
                    $scope.errorMessage = ConstService.invalidPassword;
                    onceClicked = ConstService.zero;
                } else if ($scope.passwordValue !== $scope.repeatPasswordValue) {
                    $scope.errorMessage = ConstService.invalidConfirmPassword;
                    onceClicked = ConstService.zero;
                } else if (!$scope.strengthValue) {
                    $scope.errorMessage = ConstService.invalidStrengthPassword;
                    onceClicked = ConstService.zero;
                } else if ((value.email) && (value.password)) {
                    RegisterService
                        .registerUser(value)
                        .then(function (newUserId) {
                            if (newUserId) {
                                $modalInstance.close();
                                window.location.replace('/Index#/EmailSentPage');
                            } else {
                                onceClicked = ConstService.zero;
                                $scope.errorMessage = ConstService.existUser;
                            };
                        },
                        function () {
                            onceClicked = ConstService.zero;
                            $scope.errorMessage = ConstService.existUser;
                        });
                } else {
                    onceClicked = ConstService.zero;
                    $scope.errorMessage = ConstService.inputData;
                }
            } else {
                $scope.errorMessage = ConstService.pleaseWaitMessage;
            };
    };

    $scope.setLabelColor = function() {
        var strongRegex = new RegExp('^(?=.{8,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$', 'g');
        var mediumRegex = new RegExp('^(?=.{7,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$', 'g');
        var enoughRegex = new RegExp('(?=.{6,}).*', 'g');
        if (false == enoughRegex.test($scope.passwordValue)) {
            $scope.turnRed();
            $('#passstrength').html('More Characters');
        } else if (strongRegex.test($scope.passwordValue)) {
            $scope.strengthValue = true;
            $scope.turnGreen();
            $('#passstrength').html('Strong!');
        } else if (mediumRegex.test($scope.passwordValue)) {
            $scope.strengthValue = true;
            $scope.turnYellow();
            $('#passstrength').html('Medium!');
        } else {
            $scope.turnRed();
            $('#passstrength').html('Weak!');
        }
    }
});