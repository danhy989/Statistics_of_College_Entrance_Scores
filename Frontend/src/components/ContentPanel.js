import React from 'react';
import { Col } from 'react-bootstrap';

const ContentPanel = (props) => {
    return (
        <Col md={9} className="p-0">
            <div className="content-panel">
                {props.children}
            </div>
        </Col>
    );
};

export default ContentPanel;