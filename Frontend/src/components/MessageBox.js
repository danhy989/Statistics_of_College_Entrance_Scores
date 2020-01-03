import React from 'react';
import { Modal, Button } from 'react-bootstrap';

const MessageBox = ({ show, message, handleClose }) => {
    return (
        <Modal show={show} onHide={handleClose}>
            <Modal.Body>
                {message}
            </Modal.Body>
            <Modal.Footer>
                <Button variant="primary" onClick={handleClose}>OK</Button>
            </Modal.Footer>
        </Modal>
    );
};

export default MessageBox;
