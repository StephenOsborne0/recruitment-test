﻿import axios from 'axios';

export const fetchMockEmployees = () => {
    return [
        { name: "Alice", value: 100 },
        { name: "Bob", value: 200 },
        { name: "Charlie", value: 300 }
    ];
}

export const fetchEmployees = () => {
    return axios.get('/get');
};

export const fetchEmployeesOver11171 = () => {
    return axios.get('/SumOfValuesList');
};