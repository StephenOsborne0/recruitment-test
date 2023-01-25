import React, { useState } from 'react';
import { Icon, Table, Menu, Button } from 'semantic-ui-react'
import { updateEmployees } from "./actions"

const EmployeeList = ({
  employees, 
  setEmployees,
  currentPage,
  setCurrentPage
}) => {
    const [employeesPerPage] = useState(10);
    
    const UpdateEmployeeValues = () => {
        employees.forEach(e => UpdateEmployeeValue(e));
        updateEmployees(employees);
        setEmployees(employees);
    }
    
    const UpdateEmployeeValue = (employee) => {
        if (employee.name.toUpperCase().startsWith("E")) {
            employee.value++;
        } else if (employee.name.toUpperCase().startsWith("G")) {
            employee.value += 10;
        } else {
            employee.value += 100;
        }
    };

    const indexOfLastEmployee = currentPage * employeesPerPage;
    const indexOfFirstEmployee = indexOfLastEmployee - employeesPerPage;
    const currentEmployees = employees.slice(indexOfFirstEmployee, indexOfLastEmployee);
    const pageNumbers = [];
    
    for (let i = 1; i <= Math.ceil(employees.length / employeesPerPage); i++) {
        pageNumbers.push(i);
    }

    const handleClick = (event, { name }) => setCurrentPage(name);

    return (
        <div className="ui grid">
            <div className="fourteen wide column">
                <Table celled className="employees-table">
                    <Table.Header>
                        <Table.Row>
                            <Table.HeaderCell>Name</Table.HeaderCell>
                            <Table.HeaderCell>Value</Table.HeaderCell>
                        </Table.Row>
                    </Table.Header>
                    <Table.Body>
                        {
                            currentEmployees.map((e) => MapEmployeeToTableRow(e)) 
                        }
                    </Table.Body>
                    <Table.Footer>
                        <Table.Row>
                            <Table.HeaderCell colSpan='3'>
                                <Menu floated='right' pagination>
                                    <Menu.Item as='a' icon onClick={() => {
                                        if (currentPage > 1) 
                                            setCurrentPage(currentPage - 1)
                                    }}>
                                        <Icon name='chevron left' />
                                    </Menu.Item>
                                    { pageNumbers.map(number => (
                                        <Menu.Item as='a' key={number} name={number.toString()} active={currentPage === number} onClick={handleClick}>
                                            {number}
                                        </Menu.Item>
                                    ))}
                                    <Menu.Item as='a' icon onClick={() => { 
                                        if (currentPage < pageNumbers.length) 
                                            setCurrentPage(currentPage + 1)
                                    }}>
                                        <Icon name='chevron right' />
                                    </Menu.Item>
                                </Menu>
                            </Table.HeaderCell>
                        </Table.Row>
                    </Table.Footer>
                </Table>
            </div>
            <div className="two wide column">
                <Button className="ui primary increment-button" onClick={UpdateEmployeeValues}>Increment</Button>
            </div>
        </div>
    );
};

const MapEmployeeToTableRow = (employee) => {
    return (
        //Note: the key here should be a unique id
        <Table.Row key={employee.name}>
            <Table.Cell>
                <span>{employee.name}</span>
            </Table.Cell>
            <Table.Cell>
                <span>{employee.value}</span>
            </Table.Cell>
        </Table.Row>
    );
}

export default EmployeeList;
