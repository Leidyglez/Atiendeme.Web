﻿(function () {
    angular.module('atiendeme').factory("officeRepository", officeRepository);
    //Nota: no se extraen los $http para facilitar las tareas de debug.
    function officeRepository($http) {
        function getOffices() {
            var url = "/api/Offices";
            var req = requestBuilder(url);

            return $http(req).then(
                function (response) {
                    if (response.status < 205)
                        return response.data;
                    else
                        throw response;
                }, function (error) {
                    throw error;
                });
        }

        function saveOffice(form) {
            var url = "/api/Offices";
            var req = requestBuilder(url, 'POST', form);

            return $http(req).then(
                function (response) {
                    if (response.status < 205)
                        return response.data;
                    else
                        throw response;
                }, function (error) {
                    throw error;
                });
        }

        function updateOffice(form) {
            var url = "/api/Offices";
            var req = requestBuilder(url, 'PATCH', form);

            return $http(req).then(
                function (response) {
                    if (response.status < 205)
                        return response.data;
                    else
                        throw response;
                }, function (error) {
                    throw error;
                });
        }
        function deleteOffice(id) {
            var url = "/api/Offices/" + id;
            var req = requestBuilder(url, 'DELETE');

            return $http(req).then(
                function (response) {
                    if (response.status < 205)
                        return response.data;
                    else
                        throw response;
                }, function (error) {
                    throw error;
                });
        }

        //move to tools
        function requestBuilder(_url, _method, form) {
            var req = {
                method: _method || 'GET',
                url: _url,
                headers: {
                    "accept": "application/json;odata=verbose"
                }
            };

            if (form)
                req.data = form;

            return req;
        }

        return {
            getOffices: getOffices,
            saveOffice: saveOffice,
            updateOffice: updateOffice,
            deleteOffice: deleteOffice
        };
    }
}());