app.service('IndexService',
    function (UserService) {
        this.isLoggedInto = function () {
            return Boolean(UserService.getUserData());
        };
    });