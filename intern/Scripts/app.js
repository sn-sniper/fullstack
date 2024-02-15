var app = angular.module('myApp', ['ui.bootstrap']);

app.controller('DeviceController', ['$scope', '$uibModal', 'DeviceService', function ($scope, $uibModal, DeviceService) {
    $scope.gridContainer = [];
    $scope.textfield = "";
    $scope.devicesData = [];
    $scope.createGrid = function (data) {
        $scope.gridContainer = data;
    };

    DeviceApiService.getAllDevices()
        .then(function (response) {
            $scope.devicesData = response.data;
            console.log('Devices data:', $scope.devicesData);
            $scope.createGrid($scope.devicesData);
            console.log($scope.gridContainer);
        })
        .catch(function (error) {
            console.error('Error fetching devices', error);
        });

    $scope.search = function () {
        DeviceApiService.getFilteredDevices($scope.textfield)
            .then(function (response) {
                $scope.devicesData = response.data;
                $scope.createGrid($scope.devicesData);
                console.log(response.data);
                console.log($scope.devicesData);
            })
            .catch(function (error) {
                console.error('Error fetching filtered devices', error);
            });
    };

    $scope.addDevice = function () {
        if ($scope.textfield !== "") {
            const newName = $scope.textfield;
            const newDevice = { id: $scope.devicesData.length + 1, name: newName };

            DeviceApiService.addDevice(newDevice)
                .then(function (response) {
                    $scope.devicesData.push(response.data);
                    $scope.createGrid($scope.devicesData);
                    $scope.textfield = "";
                })
                .catch(function (error) {
                    console.error('Error adding device', error);
                });
        }
    };

    $scope.addDevice = function () {
        var modalInstance = $uibModal.open({

        })
    }

    //$scope.createGrid($scope.devicesData);

    $scope.editDevice = function (index) {
        var modalInstance = $uibModal.open({
            templateUrl: 'editDeviceModal.html',
            controller: 'EditDeviceModalController',
            resolve: {
                device: function () {
                    return angular.copy($scope.devicesData[index]);
                }
            }
        });

        modalInstance.result.then(function (updatedDevice) {

            $scope.devicesData[index] = updatedDevice;
            $scope.createGrid($scope.devicesData);
            console.log(updatedDevice);
        });
    };
});

app.controller('EditDeviceModalController', function ($scope, $uibModalInstance, device) {
    $scope.updatedDevice = angular.copy(device);

    $scope.save = function () {
        $uibModalInstance.close($scope.updatedDevice);
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});

app.service('DeviceApiService', function ($http) {
    var apiUrl = '/Api/DeviceApiController';

    this.getAllDevices = function () {
        return $http.get(apiUrl);
    };

    this.getFilteredDevices = function (searchTerm) {
        return $http.get(apiUrl + '/GetFilteredDevices', { params: { searchTerm: searchTerm } });
    };

    this.addDevice = function (newDevice) {
        return $http.post(apiUrl + '/AddDevice', newDevice);
    };

    this.updateDevice = function (id, updatedDevice) {
        return $http.put(apiUrl + '/UpdateDevice/' + id, updatedDevice);
    };

    this.removeDevice = function (deviceId) {
        return $http.delete(apiUrl + '/RemoveDevice/' + deviceId);
    };
});
