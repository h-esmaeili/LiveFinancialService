
function handleResponse(response) {
    return response.text().then(text => {
        if (!response.ok) {
            if ([401, 403].indexOf(response.status) !== -1) {
                // auto logout if 401 Unauthorized or 403 Forbidden response returned from api
            }

            const error = (data && data.message) || response.statusText;
            return Promise.reject(error);
        }
        const data = text && JSON.parse(text);
        return data;
    });
}

export default handleResponse;