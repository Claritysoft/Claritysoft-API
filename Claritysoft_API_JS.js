var clarityApiFactory = {
    createApiClient: function (url, key) {
        return {
            apiBaseUrl: url,
            apiKey: key,
            apiUrl: function (relUrl) {
                var sep = relUrl.match(/\?/) ? '&' : '?';
                return this.apiBaseUrl + relUrl + sep + 'key=' + this.apiKey;
            },
            processReturnedObject: function (data) {
            },
            execAjax: function (url, type, data, callbackOk, callbackFail) {
                var t = this;
                $.ajax({
                    url: url,
                    type: type,
                    data: data,
                    success: function (data) { t.processReturnedObject(data); callbackOk(data); },
                    error: function (jq, textStatus, errorThrown) {
                        if (callbackFail != null) {
                            var exMessage = null;
                            try
                            {
                                var exceptionObj = jQuery.parseJSON(jq.responseText);
                                if (exceptionObj!=null)
                                {
                                    exMessage = exceptionObj.ExceptionMessage;
                                    if (exMessage == null || exMessage == '') {
                                        exMessage = exceptionObj.Message;
                                    }
                                }
                                
                            }
                            catch (ex) { }
                            if (exMessage == null || exMessage == '') {
                                exMessage = errorThrown;
                            }
                            callbackFail(exMessage);
                        }
                    }
                });
            },
            doDelete: function (id, callbackOk, callbackFail) {
                this.execAjax(this.apiUrl('?id=' + id), 'DELETE', '', callbackOk, callbackFail);
            },
            doUpdate: function (id, data, callbackOk, callbackFail) {
                this.execAjax(this.apiUrl('?id=' + id), 'PUT', data, callbackOk, callbackFail);

            },
            doCreate: function (data, callbackOk, callbackFail) {
                this.execAjax(this.apiUrl('/'), 'POST', data, callbackOk, callbackFail);
            },
          
            oQuery: function (oDataQueryEncoded, callbackOk, callbackFail) {
                this.execAjax(this.apiUrl('?$' + oDataQueryEncoded), 'GET', '',
                        callbackOk, callbackFail);

            },
            oQuerySub: function(subUrl, oDataQueryEncoded, callbackOk, callbackFail) {
                this.execAjax(this.apiUrl('/'+subUrl+'?$' + oDataQueryEncoded), 'GET', '',
                      callbackOk, callbackFail);
            },
            doExec: function (relUrl, type, data, callbackOk, callbackFail) {
                this.execAjax(this.apiUrl(relUrl), type, data, callbackOk, callbackFail);
            }
        };
    }

};