/* PRIMATA ARCHIVE - CLIENTE HTTP DO BACKEND */
var API_BASE_URL = window.PRIMATA_API_URL || "http://localhost:8080/api";

var PrimataAPI = {
    request: async function (endpoint, options) {
        options = options || {};
        var config = { method: options.method || "GET", headers: options.headers || {} };
        if (!(options.body instanceof FormData)) config.headers["Content-Type"] = "application/json";
        var token = localStorage.getItem("primataToken");
        if (token) config.headers["Authorization"] = "Bearer " + token;
        if (options.body !== undefined) config.body = options.body instanceof FormData ? options.body : JSON.stringify(options.body);
        var response;
        try { response = await fetch(API_BASE_URL + endpoint, config); } catch (error) { throw new Error("Não foi possível conectar ao backend em " + API_BASE_URL + "."); }
        var type = response.headers.get("content-type") || "";
        var data = type.includes("application/json") ? await response.json() : await response.text();
        if (response.status === 401) { localStorage.removeItem("primataToken"); localStorage.removeItem("primataUsuario"); }
        if (!response.ok) {
            var message = data && data.message ? data.message : (typeof data === "string" && data ? data : "Erro na API (HTTP " + response.status + ").");
            throw new Error(message);
        }
        return data;
    },
    get: function (endpoint) { return this.request(endpoint); },
    post: function (endpoint, body) { return this.request(endpoint, { method: "POST", body: body }); },
    put: function (endpoint, body) { return this.request(endpoint, { method: "PUT", body: body }); },
    patch: function (endpoint, body) { return this.request(endpoint, { method: "PATCH", body: body }); },
    delete: function (endpoint) { return this.request(endpoint, { method: "DELETE" }); },
    upload: function (endpoint, formData, method) { return this.request(endpoint, { method: method || "POST", body: formData }); }
};

function mostrarErroAPI(error) { console.error(error); alert(error.message || "Ocorreu um erro."); }
