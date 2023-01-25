import React, { useEffect, useState } from 'react';
import { EmployeeList } from "./Components/EmployeeList";
import { Tab } from "semantic-ui-react";
import { fetchEmployees } from "./Components/EmployeeList/actions";

const App = () => {
    
    const [employees, setEmployees] = useState(null);
    const [summedEmployees, setSummedEmployees] = useState(null);
    const [tabIndex, setTabIndex] = useState(0);
    const [currentPage, setCurrentPage] = useState(1);

    useEffect(() => {
        fetchEmployees().then(response => {
            setEmployees(response.data);
            var sum = employeesOver11171(response.data);
            setSummedEmployees(sum);
        });
    }, []);
    
    useEffect(() => {
        setCurrentPage(1);
    }, [tabIndex])

    const employeesOver11171 = (employees) => {
        const employeesToSum = employees.filter(e =>
            e.name.toUpperCase().startsWith("A") ||
            e.name.toUpperCase().startsWith("B") ||
            e.name.toUpperCase().startsWith("C")
        );

        //Calculate this but do not display it...?
        const abcSum = employeesToSum.map(e => e.value).reduce((acc, val) => acc + val);
        console.log(`ABC sum = ${abcSum}`);

        let sum = 0;
        return employeesToSum.filter(employee => {
            if (sum >= 11171) {
                return true;
            } else {
                sum += employee.value;
                return false;
            }
        });
    }
    
    const panes = [
        {
            menuItem: 'Full Employee List', render: () =>
                <Tab.Pane>
                    { employees && employees.length > 0
                        ? <EmployeeList
                            employees={employees} 
                            setEmployees={() => setEmployees}
                            currentPage={currentPage}
                            setCurrentPage={setCurrentPage}
                        />
                        : "Loading..." }
                </Tab.Pane>
        },
        {
            menuItem: 'Partial Employee List', render: () =>
                <Tab.Pane>
                    { summedEmployees && summedEmployees.length > 0
                        ? <EmployeeList
                            employees={summedEmployees}
                            setEmployees={() => setSummedEmployees}
                            currentPage={currentPage}
                            setCurrentPage={() => setCurrentPage}
                        />
                        : "Loading..." }
                </Tab.Pane>
        }
    ];

    return (
        <div className="ui container">
            <h2 className="employee-list-header">Employees</h2>
            <div className="employee-list">
                <Tab panes={panes} activeIndex={tabIndex} onTabChange={(e, data) => setTabIndex(data.activeIndex)}/>
            </div>
        </div>
    );
};

export default App;