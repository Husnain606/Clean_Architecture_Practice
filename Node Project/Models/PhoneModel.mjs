import mongoose from 'mongoose';

const phoneSchema = mongoose.Schema({
    id: {
        type: Number,
        required: true  // fixed the typo here as well
    },
    name: {
        type: String,
        required: true
    },
    color: {
        type: String,
        required: true
    },
    ROM: {
        type: String,
        required: true
    },
    price: {
        type: Number,
        required: true
    },
    camera: {
        type: String,
        required: true
    },
    modelName: {
        type: String,
        required: true
    },
    modelNumber: {
        type: Number,
        required: true
    },
    size: {
        type: String,
        required: true
    },
    Description: {
        type: String,
        required: true
    },
    productImage: {
        type: String,
        required: true
    },
});

const phone = mongoose.model('iphones', phoneSchema);
export default phone;
