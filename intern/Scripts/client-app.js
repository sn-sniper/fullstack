var app = angular.module('client-app', []);

app.controller('ClientController', function ($scope, $http) {
    $scope.client = {
        id: null,
        name: '',
        type: '',
        birthDate: null
    };

    $scope.submitForm = function () {
        var requestData = {
            name: $scope.client.name,
            type: $scope.client.type,
            birthDate: $scope.client.birthDate
        };

        $http.post('/api/clients', requestData)
            .then(function (response) {
                console.log('Client added successfully:', response.data);
                $scope.resetForm();
            })
            .catch(function (error) {
                console.error('Error adding client:', error);
            });
    };

    $scope.resetForm = function () {
        $scope.client = {
            id: null,
            name: '',
            type: '',
            birthDate: null
        };
    };
});