import Phone from '../models/phoneModel.mjs';

// Create a new phone
export const createPhone = async (req, res) => {
    try {
        const newPhone = await Phone.create(req.body);
     res.status(200).json({
        status:'success',
        mobile :newPhone
     })
    
    } catch (error) {
        res.status(400).send(`
            <h1>Error</h1>
            <p>Could not create phone: ${error.message}</p>
            <a href="/">Go Back</a>
        `);
    }
};

// Get all phones
export const getAllPhones = async (req, res) => {
    try {
        const phones = await Phone.find();
        res.status(200).json({
            status:'success',
            mobile :{phones}
         })
    } catch (error) {
        res.status(400).send(`
            <h1>Error</h1>
            <p>Could not retrieve phones: ${error.message}</p>
            <a href="/">Go Back</a>
        `);
    }
};

// Get a specific phone
export const getPhone = async (req, res) => {
    try {
        const phone = await Phone.findById(req.params.id);
        if (!phone) {
            res.status(404).send(`
                <h1>Phone Not Found</h1>
                <p>No phone found with that ID.</p>
                <a href="/">Go Back</a>
            `);
            return;
        }
        res.status(200).json({
            status:'success',
            mobile :phone
         })
    } catch (error) {
        res.status(400).send(`
            <h1>Error</h1>
            <p>Could not retrieve phone: ${error.message}</p>
            <a href="/">Go Back</a>
        `);
    }
};

// Update a phone
export const updatePhone = async (req, res) => {
    try {
        const phone = await Phone.findByIdAndUpdate(req.params.id, req.body, {
            new: true,
            runValidators: true,
        });
        if (!phone) {
            res.status(404).send(`
                <h1>Phone Not Found</h1>
                <p>No phone found with that ID.</p>
                <a href="/">Go Back</a>
            `);
            return;
        }
        res.status(200).json({
            status:'success',
            mobile :phone
         })
    } catch (error) {
        res.status(400).send(`
            <h1>Error</h1>
            <p>Could not update phone: ${error.message}</p>
            <a href="/">Go Back</a>
        `);
    }
};

// Delete a phone
export const deletePhone = async (req, res) => {
    try {
        const phone = await Phone.findByIdAndDelete(req.params.id);
        if (!phone) {
            res.status(404).send(`
                <h1>Phone Not Found</h1>
                <p>No phone found with that ID.</p>
                <a href="/">Go Back</a>
            `);
            return;
        }
        res.status(200).json({
            status:'success',
            mobile :phone
         })
    } catch (error) {
        res.status(400).send(`
            <h1>Error</h1>
            <p>Could not delete phone: ${error.message}</p>
            <a href="/">Go Back</a>
        `);
    }
};
