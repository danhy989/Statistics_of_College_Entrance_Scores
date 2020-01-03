import React from 'react';
import { Container, Row, Col, Navbar, Nav } from 'react-bootstrap';
import FontAwesome from 'react-fontawesome';

const Header = () => {
    return (
        <div className="header">
            <Container fluid>
                <Row>
                    <Col className="p-0">
                        <Navbar expand="lg" bg="dark" variant="dark">
                            <Navbar.Brand href="/">Tra cứu điểm chuẩn</Navbar.Brand>
                            <Nav className="ml-auto mr-3">
                                <Nav.Link href="#about">
                                    Giới thiệu
                                </Nav.Link>
                                <Nav.Link href="http://35.240.228.120:5000/swagger/index.html" target="_blank">
                                    Swagger
                                    <FontAwesome name="external-link" className="pl-2" />
                                </Nav.Link>
                            </Nav>
                        </Navbar>
                    </Col>
                </Row>
            </Container>
        </div>
    );
};

export default Header;
