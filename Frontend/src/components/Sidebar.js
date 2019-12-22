import React from 'react';
import { Col, Card, Accordion } from 'react-bootstrap';
import { Link } from 'react-router-dom';

const Sidebar = ({ children }) => {
    return (
        <Col md={3} className="p-0">
            <Accordion className="sidebar">
                {children}
            </Accordion>
        </Col>
    );
};

Sidebar.Item = ({ link, title, eventKey, children }) => {
    if (children) {
        return (
            <Card className="sidebar-item">
                <Accordion.Toggle
                    as={Card.Header}
                    className="sidebar-item-title"
                    eventKey={eventKey}
                    style={{ cursor: 'pointer' }}
                >
                    {title}
                </Accordion.Toggle>
                <Accordion.Collapse eventKey={eventKey}>
                    <Card.Body>
                        {children}
                    </Card.Body>
                </Accordion.Collapse>
            </Card>
        );
    }

    return (
        <Card className="sidebar-item">
            <Card.Header>
                <Link to={link}>
                    {title}
                </Link>
            </Card.Header>
        </Card>
    );
};

Sidebar.Subitem = ({ link, title }) => {
    return (
        <div className="sidebar-subitem">
            <Link to={link}>
                {title}
            </Link>
        </div>
    );
};

export default Sidebar;
