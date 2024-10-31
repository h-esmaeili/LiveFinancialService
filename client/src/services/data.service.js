import { handleResponse } from '../helpers';
const apiUrl = 'http://localhost:50600';

const DataService = {
    get
}

async function get(url) {
    const headers = { 'Content-Type': 'application/json' };
    const options = { method: 'GET', headers };
    console.log('get data from server.')
    return await fetch(`${apiUrl}/${url}`, options);
}

export default DataService;