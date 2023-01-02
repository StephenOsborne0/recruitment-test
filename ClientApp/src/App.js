import React, { useState, useEffect } from 'react';
import {fetchEmployees, fetchEmployeesOver11171, fetchMockEmployees} from './actions';

const App = (props) => {
    
    const [employees, setEmployees] = useState(null);
    const [summedEmployees, setSummedEmployees] = useState(null);

    useEffect(() => {
        fetchEmployees().then(response => setEmployees(response.data));
        fetchEmployeesOver11171().then(response => setSummedEmployees(response.data));
    }, []);
    
    return (
        <div>
            <div>
                <h1>Employee List</h1>
                {employees && employees.length > 0 &&
                    <ul>
                        {employees.map((employee, index) => (
                            <li key={index}>
                                {employee.name}: {employee.value}
                            </li>
                        ))}
                    </ul>
                }
            </div>
    
            <br/>
            <hr/>
    
            <div>
                <h1>Over 11171</h1>
                {summedEmployees && summedEmployees.length > 0 &&
                    <ul>
                        {summedEmployees.map((employee, index) => (
                            <li key={index}>
                                {employee.name}: {employee.value}
                            </li>
                        ))}
                    </ul>
                }
            </div>
        </div>
    );
};

export default App;
