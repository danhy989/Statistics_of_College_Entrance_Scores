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
                                <Nav.Link href="https://github.com/danhy989/Statistics_of_College_Entrance_Scores" target="_blank">
                                    GitHub
                                    <FontAwesome name="external-link" className="pl-1" />
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