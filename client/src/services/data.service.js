import { authHeader, handleResponse } from '../helpers';
const apiUrl = 'http://localhost:50600';

const DataService = {
    get
}

function get(url) {
    const headers = { 'Content-Type': 'application/json' };
    const options = { method: 'GET', headers };
    console.log('get data from server.')
    return fetch(`${apiUrl}/${url}`, options)
        .then(handleResponse)
        .then(
            response => {
                console.log('response:', response)
                if(response.success === false) {
                    const error = (response && response.message) || response.statusText;
                    return Promise.reject(error);
                }   
                return response;           
            }
        )
}

export default DataService;