import React from 'react';
import { Container, Row, Col, Navbar, Nav } from 'react-bootstrap';

const Header = () => {
    return (
        <div className="header">
            <Container fluid>
                <Row>
                    <Col className="p-0">
                        <Navbar expand="lg" bg="dark" variant="dark">
                            <Nav className="ml-auto mr-3">
                                <Nav.Link href="/home">Abc</Nav.Link>
                                <Nav.Link href="/home">Xyz</Nav.Link>
                            </Nav>
                        </Navbar>
                    </Col>
                </Row>
            </Container>
        </div>
    );
};

export default Header;