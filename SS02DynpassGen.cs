using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mcprog
{
    class SS02DynpassGen : ARM_Emulator
    {
        byte[] _secretSeed = new byte[8];
        
        byte[] _writePass = new byte[3];

        public byte[] SecretSeed
        {
            get{return _secretSeed;}
        }
        
        public byte[] WritePass
        {
            get { return _writePass; }
        }

        private void MemoryInit()
        {
            memory = new Dictionary<uint, byte>();
            byte[] sc23 = new byte[4096] { 0x29, 0x89, 0xA1, 0xA8, 0x05, 0x85, 0x81, 0x84, 0x16, 0xC6, 0xD2, 0xD4, 0x13, 0xC3, 0xD3, 0xD0, 0x14, 0x44, 0x50, 0x54, 0x1D, 0x0D, 0x11, 0x1C, 0x2C, 0x8C, 0xA0, 0xAC, 0x25, 0x05, 0x21, 0x24, 0x1D, 0x4D, 0x51, 0x5C, 0x03, 0x43, 0x43, 0x40, 0x18, 0x08, 0x10, 0x18, 0x1E, 0x0E, 0x12, 0x1C, 0x11, 0x41, 0x51, 0x50, 0x3C, 0xCC, 0xF0, 0xFC, 0x0A, 0xCA, 0xC2, 0xC8, 0x23, 0x43, 0x63, 0x60, 0x28, 0x08, 0x20, 0x28, 0x04, 0x44, 0x40, 0x44, 0x20, 0x00, 0x20, 0x20, 0x1D, 0x8D, 0x91, 0x9C, 0x20, 0xC0, 0xE0, 0xE0, 0x22, 0xC2, 0xE2, 0xE0, 0x08, 0xC8, 0xC0, 0xC8, 0x17, 0x07, 0x13, 0x14, 0x25, 0x85, 0xA1, 0xA4, 0x0F, 0x8F, 0x83, 0x8C, 0x03, 0x03, 0x03, 0x00, 0x3B, 0x4B, 0x73, 0x78, 0x3B, 0x8B, 0xB3, 0xB8, 0x13, 0x03, 0x13, 0x10, 0x12, 0xC2, 0xD2, 0xD0, 0x2E, 0xCE, 0xE2, 0xEC, 0x30, 0x40, 0x70, 0x70, 0x0C, 0x8C, 0x80, 0x8C, 0x3F, 0x0F, 0x33, 0x3C, 0x28, 0x88, 0xA0, 0xA8, 0x32, 0x02, 0x32, 0x30, 0x1D, 0xCD, 0xD1, 0xDC, 0x36, 0xC6, 0xF2, 0xF4, 0x34, 0x44, 0x70, 0x74, 0x2C, 0xCC, 0xE0, 0xEC, 0x15, 0x85, 0x91, 0x94, 0x0B, 0x0B, 0x03, 0x08, 0x17, 0x47, 0x53, 0x54, 0x1C, 0x4C, 0x50, 0x5C, 0x1B, 0x4B, 0x53, 0x58, 0x3D, 0x8D, 0xB1, 0xBC, 0x01, 0x01, 0x01, 0x00, 0x24, 0x04, 0x20, 0x24, 0x1C, 0x0C, 0x10, 0x1C, 0x33, 0x43, 0x73, 0x70, 0x18, 0x88, 0x90, 0x98, 0x10, 0x00, 0x10, 0x10, 0x0C, 0xCC, 0xC0, 0xCC, 0x32, 0xC2, 0xF2, 0xF0, 0x19, 0xC9, 0xD1, 0xD8, 0x2C, 0x0C, 0x20, 0x2C, 0x27, 0xC7, 0xE3, 0xE4, 0x32, 0x42, 0x72, 0x70, 0x03, 0x83, 0x83, 0x80, 0x1B, 0x8B, 0x93, 0x98, 0x11, 0xC1, 0xD1, 0xD0, 0x06, 0x86, 0x82, 0x84, 0x09, 0xC9, 0xC1, 0xC8, 0x20, 0x40, 0x60, 0x60, 0x10, 0x40, 0x50, 0x50, 0x23, 0x83, 0xA3, 0xA0, 0x2B, 0xCB, 0xE3, 0xE8, 0x0D, 0x0D, 0x01, 0x0C, 0x36, 0x86, 0xB2, 0xB4, 0x1E, 0x8E, 0x92, 0x9C, 0x0F, 0x4F, 0x43, 0x4C, 0x37, 0x87, 0xB3, 0xB4, 0x1A, 0x4A, 0x52, 0x58, 0x06, 0xC6, 0xC2, 0xC4, 0x38, 0x48, 0x70, 0x78, 0x26, 0x86, 0xA2, 0xA4, 0x12, 0x02, 0x12, 0x10, 0x2F, 0x8F, 0xA3, 0xAC, 0x15, 0xC5, 0xD1, 0xD4, 0x21, 0x41, 0x61, 0x60, 0x03, 0xC3, 0xC3, 0xC0, 0x34, 0x84, 0xB0, 0xB4, 0x01, 0x41, 0x41, 0x40, 0x12, 0x42, 0x52, 0x50, 0x3D, 0x4D, 0x71, 0x7C, 0x0D, 0x8D, 0x81, 0x8C, 0x08, 0x08, 0x00, 0x08, 0x1F, 0x0F, 0x13, 0x1C, 0x19, 0x89, 0x91, 0x98, 0x00, 0x00, 0x00, 0x00, 0x19, 0x09, 0x11, 0x18, 0x04, 0x04, 0x00, 0x04, 0x13, 0x43, 0x53, 0x50, 0x37, 0xC7, 0xF3, 0xF4, 0x21, 0xC1, 0xE1, 0xE0, 0x3D, 0xCD, 0xF1, 0xFC, 0x36, 0x46, 0x72, 0x74, 0x2F, 0x0F, 0x23, 0x2C, 0x27, 0x07, 0x23, 0x24, 0x30, 0x80, 0xB0, 0xB0, 0x0B, 0x8B, 0x83, 0x88, 0x0E, 0x0E, 0x02, 0x0C, 0x2B, 0x8B, 0xA3, 0xA8, 0x22, 0x82, 0xA2, 0xA0, 0x2E, 0x4E, 0x62, 0x6C, 0x13, 0x83, 0x93, 0x90, 0x0D, 0x4D, 0x41, 0x4C, 0x29, 0x49, 0x61, 0x68, 0x3C, 0x4C, 0x70, 0x7C, 0x09, 0x09, 0x01, 0x08, 0x0A, 0x0A, 0x02, 0x08, 0x3F, 0x8F, 0xB3, 0xBC, 0x2F, 0xCF, 0xE3, 0xEC, 0x33, 0xC3, 0xF3, 0xF0, 0x05, 0xC5, 0xC1, 0xC4, 0x07, 0x87, 0x83, 0x84, 0x14, 0x04, 0x10, 0x14, 0x3E, 0xCE, 0xF2, 0xFC, 0x24, 0x44, 0x60, 0x64, 0x1E, 0xCE, 0xD2, 0xDC, 0x2E, 0x0E, 0x22, 0x2C, 0x0B, 0x4B, 0x43, 0x48, 0x1A, 0x0A, 0x12, 0x18, 0x06, 0x06, 0x02, 0x04, 0x21, 0x01, 0x21, 0x20, 0x2B, 0x4B, 0x63, 0x68, 0x26, 0x46, 0x62, 0x64, 0x02, 0x02, 0x02, 0x00, 0x35, 0xC5, 0xF1, 0xF4, 0x12, 0x82, 0x92, 0x90, 0x0A, 0x8A, 0x82, 0x88, 0x0C, 0x0C, 0x00, 0x0C, 0x33, 0x83, 0xB3, 0xB0, 0x3E, 0x4E, 0x72, 0x7C, 0x10, 0xC0, 0xD0, 0xD0, 0x3A, 0x4A, 0x72, 0x78, 0x07, 0x47, 0x43, 0x44, 0x16, 0x86, 0x92, 0x94, 0x25, 0xC5, 0xE1, 0xE4, 0x26, 0x06, 0x22, 0x24, 0x00, 0x80, 0x80, 0x80, 0x2D, 0x8D, 0xA1, 0xAC, 0x1F, 0xCF, 0xD3, 0xDC, 0x21, 0x81, 0xA1, 0xA0, 0x30, 0x00, 0x30, 0x30, 0x37, 0x07, 0x33, 0x34, 0x2E, 0x8E, 0xA2, 0xAC, 0x36, 0x06, 0x32, 0x34, 0x15, 0x05, 0x11, 0x14, 0x22, 0x02, 0x22, 0x20, 0x38, 0x08, 0x30, 0x38, 0x34, 0xC4, 0xF0, 0xF4, 0x27, 0x87, 0xA3, 0xA4, 0x05, 0x45, 0x41, 0x44, 0x0C, 0x4C, 0x40, 0x4C, 0x01, 0x81, 0x81, 0x80, 0x29, 0xC9, 0xE1, 0xE8, 0x04, 0x84, 0x80, 0x84, 0x17, 0x87, 0x93, 0x94, 0x35, 0x05, 0x31, 0x34, 0x0B, 0xCB, 0xC3, 0xC8, 0x0E, 0xCE, 0xC2, 0xCC, 0x3C, 0x0C, 0x30, 0x3C, 0x31, 0x41, 0x71, 0x70, 0x11, 0x01, 0x11, 0x10, 0x07, 0xC7, 0xC3, 0xC4, 0x09, 0x89, 0x81, 0x88, 0x35, 0x45, 0x71, 0x74, 0x3B, 0xCB, 0xF3, 0xF8, 0x1A, 0xCA, 0xD2, 0xD8, 0x38, 0xC8, 0xF0, 0xF8, 0x14, 0x84, 0x90, 0x94, 0x19, 0x49, 0x51, 0x58, 0x02, 0x82, 0x82, 0x80, 0x04, 0xC4, 0xC0, 0xC4, 0x3F, 0xCF, 0xF3, 0xFC, 0x09, 0x49, 0x41, 0x48, 0x39, 0x09, 0x31, 0x38, 0x27, 0x47, 0x63, 0x64, 0x00, 0xC0, 0xC0, 0xC0, 0x0F, 0xCF, 0xC3, 0xCC, 0x17, 0xC7, 0xD3, 0xD4, 0x38, 0x88, 0xB0, 0xB8, 0x0F, 0x0F, 0x03, 0x0C, 0x0E, 0x8E, 0x82, 0x8C, 0x02, 0x42, 0x42, 0x40, 0x23, 0x03, 0x23, 0x20, 0x11, 0x81, 0x91, 0x90, 0x2C, 0x4C, 0x60, 0x6C, 0x1B, 0xCB, 0xD3, 0xD8, 0x24, 0x84, 0xA0, 0xA4, 0x34, 0x04, 0x30, 0x34, 0x31, 0xC1, 0xF1, 0xF0, 0x08, 0x48, 0x40, 0x48, 0x02, 0xC2, 0xC2, 0xC0, 0x2F, 0x4F, 0x63, 0x6C, 0x3D, 0x0D, 0x31, 0x3C, 0x2D, 0x0D, 0x21, 0x2C, 0x00, 0x40, 0x40, 0x40, 0x3E, 0x8E, 0xB2, 0xBC, 0x3E, 0x0E, 0x32, 0x3C, 0x3C, 0x8C, 0xB0, 0xBC, 0x01, 0xC1, 0xC1, 0xC0, 0x2A, 0x8A, 0xA2, 0xA8, 0x3A, 0x8A, 0xB2, 0xB8, 0x0E, 0x4E, 0x42, 0x4C, 0x15, 0x45, 0x51, 0x54, 0x3B, 0x0B, 0x33, 0x38, 0x1C, 0xCC, 0xD0, 0xDC, 0x28, 0x48, 0x60, 0x68, 0x3F, 0x4F, 0x73, 0x7C, 0x1C, 0x8C, 0x90, 0x9C, 0x18, 0xC8, 0xD0, 0xD8, 0x0A, 0x4A, 0x42, 0x48, 0x16, 0x46, 0x52, 0x54, 0x37, 0x47, 0x73, 0x74, 0x20, 0x80, 0xA0, 0xA0, 0x2D, 0xCD, 0xE1, 0xEC, 0x06, 0x46, 0x42, 0x44, 0x35, 0x85, 0xB1, 0xB4, 0x2B, 0x0B, 0x23, 0x28, 0x25, 0x45, 0x61, 0x64, 0x3A, 0xCA, 0xF2, 0xF8, 0x23, 0xC3, 0xE3, 0xE0, 0x39, 0x89, 0xB1, 0xB8, 0x31, 0x81, 0xB1, 0xB0, 0x1F, 0x8F, 0x93, 0x9C, 0x1E, 0x4E, 0x52, 0x5C, 0x39, 0xC9, 0xF1, 0xF8, 0x26, 0xC6, 0xE2, 0xE4, 0x32, 0x82, 0xB2, 0xB0, 0x31, 0x01, 0x31, 0x30, 0x2A, 0xCA, 0xE2, 0xE8, 0x2D, 0x4D, 0x61, 0x6C, 0x1F, 0x4F, 0x53, 0x5C, 0x24, 0xC4, 0xE0, 0xE4, 0x30, 0xC0, 0xF0, 0xF0, 0x0D, 0xCD, 0xC1, 0xCC, 0x08, 0x88, 0x80, 0x88, 0x16, 0x06, 0x12, 0x14, 0x3A, 0x0A, 0x32, 0x38, 0x18, 0x48, 0x50, 0x58, 0x14, 0xC4, 0xD0, 0xD4, 0x22, 0x42, 0x62, 0x60, 0x29, 0x09, 0x21, 0x28, 0x07, 0x07, 0x03, 0x04, 0x33, 0x03, 0x33, 0x30, 0x28, 0xC8, 0xE0, 0xE8, 0x1B, 0x0B, 0x13, 0x18, 0x05, 0x05, 0x01, 0x04, 0x39, 0x49, 0x71, 0x78, 0x10, 0x80, 0x90, 0x90, 0x2A, 0x4A, 0x62, 0x68, 0x2A, 0x0A, 0x22, 0x28, 0x1A, 0x8A, 0x92, 0x98, 0x38, 0x38, 0x08, 0x30, 0xE8, 0x28, 0xC8, 0xE0, 0x2C, 0x2D, 0x0D, 0x21, 0xA4, 0x26, 0x86, 0xA2, 0xCC, 0x0F, 0xCF, 0xC3, 0xDC, 0x1E, 0xCE, 0xD2, 0xB0, 0x33, 0x83, 0xB3, 0xB8, 0x38, 0x88, 0xB0, 0xAC, 0x2F, 0x8F, 0xA3, 0x60, 0x20, 0x40, 0x60, 0x54, 0x15, 0x45, 0x51, 0xC4, 0x07, 0xC7, 0xC3, 0x44, 0x04, 0x44, 0x40, 0x6C, 0x2F, 0x4F, 0x63, 0x68, 0x2B, 0x4B, 0x63, 0x58, 0x1B, 0x4B, 0x53, 0xC0, 0x03, 0xC3, 0xC3, 0x60, 0x22, 0x42, 0x62, 0x30, 0x33, 0x03, 0x33, 0xB4, 0x35, 0x85, 0xB1, 0x28, 0x29, 0x09, 0x21, 0xA0, 0x20, 0x80, 0xA0, 0xE0, 0x22, 0xC2, 0xE2, 0xA4, 0x27, 0x87, 0xA3, 0xD0, 0x13, 0xC3, 0xD3, 0x90, 0x11, 0x81, 0x91, 0x10, 0x11, 0x01, 0x11, 0x04, 0x06, 0x06, 0x02, 0x1C, 0x1C, 0x0C, 0x10, 0xBC, 0x3C, 0x8C, 0xB0, 0x34, 0x36, 0x06, 0x32, 0x48, 0x0B, 0x4B, 0x43, 0xEC, 0x2F, 0xCF, 0xE3, 0x88, 0x08, 0x88, 0x80, 0x6C, 0x2C, 0x4C, 0x60, 0xA8, 0x28, 0x88, 0xA0, 0x14, 0x17, 0x07, 0x13, 0xC4, 0x04, 0xC4, 0xC0, 0x14, 0x16, 0x06, 0x12, 0xF4, 0x34, 0xC4, 0xF0, 0xC0, 0x02, 0xC2, 0xC2, 0x44, 0x05, 0x45, 0x41, 0xE0, 0x21, 0xC1, 0xE1, 0xD4, 0x16, 0xC6, 0xD2, 0x3C, 0x3F, 0x0F, 0x33, 0x3C, 0x3D, 0x0D, 0x31, 0x8C, 0x0E, 0x8E, 0x82, 0x98, 0x18, 0x88, 0x90, 0x28, 0x28, 0x08, 0x20, 0x4C, 0x0E, 0x4E, 0x42, 0xF4, 0x36, 0xC6, 0xF2, 0x3C, 0x3E, 0x0E, 0x32, 0xA4, 0x25, 0x85, 0xA1, 0xF8, 0x39, 0xC9, 0xF1, 0x0C, 0x0D, 0x0D, 0x01, 0xDC, 0x1F, 0xCF, 0xD3, 0xD8, 0x18, 0xC8, 0xD0, 0x28, 0x2B, 0x0B, 0x23, 0x64, 0x26, 0x46, 0x62, 0x78, 0x3A, 0x4A, 0x72, 0x24, 0x27, 0x07, 0x23, 0x2C, 0x2F, 0x0F, 0x23, 0xF0, 0x31, 0xC1, 0xF1, 0x70, 0x32, 0x42, 0x72, 0x40, 0x02, 0x42, 0x42, 0xD4, 0x14, 0xC4, 0xD0, 0x40, 0x01, 0x41, 0x41, 0xC0, 0x00, 0xC0, 0xC0, 0x70, 0x33, 0x43, 0x73, 0x64, 0x27, 0x47, 0x63, 0xAC, 0x2C, 0x8C, 0xA0, 0x88, 0x0B, 0x8B, 0x83, 0xF4, 0x37, 0xC7, 0xF3, 0xAC, 0x2D, 0x8D, 0xA1, 0x80, 0x00, 0x80, 0x80, 0x1C, 0x1F, 0x0F, 0x13, 0xC8, 0x0A, 0xCA, 0xC2, 0x2C, 0x2C, 0x0C, 0x20, 0xA8, 0x2A, 0x8A, 0xA2, 0x34, 0x34, 0x04, 0x30, 0xD0, 0x12, 0xC2, 0xD2, 0x08, 0x0B, 0x0B, 0x03, 0xEC, 0x2E, 0xCE, 0xE2, 0xE8, 0x29, 0xC9, 0xE1, 0x5C, 0x1D, 0x4D, 0x51, 0x94, 0x14, 0x84, 0x90, 0x18, 0x18, 0x08, 0x10, 0xF8, 0x38, 0xC8, 0xF0, 0x54, 0x17, 0x47, 0x53, 0xAC, 0x2E, 0x8E, 0xA2, 0x08, 0x08, 0x08, 0x00, 0xC4, 0x05, 0xC5, 0xC1, 0x10, 0x13, 0x03, 0x13, 0xCC, 0x0D, 0xCD, 0xC1, 0x84, 0x06, 0x86, 0x82, 0xB8, 0x39, 0x89, 0xB1, 0xFC, 0x3F, 0xCF, 0xF3, 0x7C, 0x3D, 0x4D, 0x71, 0xC0, 0x01, 0xC1, 0xC1, 0x30, 0x31, 0x01, 0x31, 0xF4, 0x35, 0xC5, 0xF1, 0x88, 0x0A, 0x8A, 0x82, 0x68, 0x2A, 0x4A, 0x62, 0xB0, 0x31, 0x81, 0xB1, 0xD0, 0x11, 0xC1, 0xD1, 0x20, 0x20, 0x00, 0x20, 0xD4, 0x17, 0xC7, 0xD3, 0x00, 0x02, 0x02, 0x02, 0x20, 0x22, 0x02, 0x22, 0x04, 0x04, 0x04, 0x00, 0x68, 0x28, 0x48, 0x60, 0x70, 0x31, 0x41, 0x71, 0x04, 0x07, 0x07, 0x03, 0xD8, 0x1B, 0xCB, 0xD3, 0x9C, 0x1D, 0x8D, 0x91, 0x98, 0x19, 0x89, 0x91, 0x60, 0x21, 0x41, 0x61, 0xBC, 0x3E, 0x8E, 0xB2, 0xE4, 0x26, 0xC6, 0xE2, 0x58, 0x19, 0x49, 0x51, 0xDC, 0x1D, 0xCD, 0xD1, 0x50, 0x11, 0x41, 0x51, 0x90, 0x10, 0x80, 0x90, 0xDC, 0x1C, 0xCC, 0xD0, 0x98, 0x1A, 0x8A, 0x92, 0xA0, 0x23, 0x83, 0xA3, 0xA8, 0x2B, 0x8B, 0xA3, 0xD0, 0x10, 0xC0, 0xD0, 0x80, 0x01, 0x81, 0x81, 0x0C, 0x0F, 0x0F, 0x03, 0x44, 0x07, 0x47, 0x43, 0x18, 0x1A, 0x0A, 0x12, 0xE0, 0x23, 0xC3, 0xE3, 0xEC, 0x2C, 0xCC, 0xE0, 0x8C, 0x0D, 0x8D, 0x81, 0xBC, 0x3F, 0x8F, 0xB3, 0x94, 0x16, 0x86, 0x92, 0x78, 0x3B, 0x4B, 0x73, 0x5C, 0x1C, 0x4C, 0x50, 0xA0, 0x22, 0x82, 0xA2, 0xA0, 0x21, 0x81, 0xA1, 0x60, 0x23, 0x43, 0x63, 0x20, 0x23, 0x03, 0x23, 0x4C, 0x0D, 0x4D, 0x41, 0xC8, 0x08, 0xC8, 0xC0, 0x9C, 0x1E, 0x8E, 0x92, 0x9C, 0x1C, 0x8C, 0x90, 0x38, 0x3A, 0x0A, 0x32, 0x0C, 0x0C, 0x0C, 0x00, 0x2C, 0x2E, 0x0E, 0x22, 0xB8, 0x3A, 0x8A, 0xB2, 0x6C, 0x2E, 0x4E, 0x62, 0x9C, 0x1F, 0x8F, 0x93, 0x58, 0x1A, 0x4A, 0x52, 0xF0, 0x32, 0xC2, 0xF2, 0x90, 0x12, 0x82, 0x92, 0xF0, 0x33, 0xC3, 0xF3, 0x48, 0x09, 0x49, 0x41, 0x78, 0x38, 0x48, 0x70, 0xCC, 0x0C, 0xCC, 0xC0, 0x14, 0x15, 0x05, 0x11, 0xF8, 0x3B, 0xCB, 0xF3, 0x70, 0x30, 0x40, 0x70, 0x74, 0x35, 0x45, 0x71, 0x7C, 0x3F, 0x4F, 0x73, 0x34, 0x35, 0x05, 0x31, 0x10, 0x10, 0x00, 0x10, 0x00, 0x03, 0x03, 0x03, 0x64, 0x24, 0x44, 0x60, 0x6C, 0x2D, 0x4D, 0x61, 0xC4, 0x06, 0xC6, 0xC2, 0x74, 0x34, 0x44, 0x70, 0xD4, 0x15, 0xC5, 0xD1, 0xB4, 0x34, 0x84, 0xB0, 0xE8, 0x2A, 0xCA, 0xE2, 0x08, 0x09, 0x09, 0x01, 0x74, 0x36, 0x46, 0x72, 0x18, 0x19, 0x09, 0x11, 0xFC, 0x3E, 0xCE, 0xF2, 0x40, 0x00, 0x40, 0x40, 0x10, 0x12, 0x02, 0x12, 0xE0, 0x20, 0xC0, 0xE0, 0xBC, 0x3D, 0x8D, 0xB1, 0x04, 0x05, 0x05, 0x01, 0xF8, 0x3A, 0xCA, 0xF2, 0x00, 0x01, 0x01, 0x01, 0xF0, 0x30, 0xC0, 0xF0, 0x28, 0x2A, 0x0A, 0x22, 0x5C, 0x1E, 0x4E, 0x52, 0xA8, 0x29, 0x89, 0xA1, 0x54, 0x16, 0x46, 0x52, 0x40, 0x03, 0x43, 0x43, 0x84, 0x05, 0x85, 0x81, 0x14, 0x14, 0x04, 0x10, 0x88, 0x09, 0x89, 0x81, 0x98, 0x1B, 0x8B, 0x93, 0xB0, 0x30, 0x80, 0xB0, 0xE4, 0x25, 0xC5, 0xE1, 0x48, 0x08, 0x48, 0x40, 0x78, 0x39, 0x49, 0x71, 0x94, 0x17, 0x87, 0x93, 0xFC, 0x3C, 0xCC, 0xF0, 0x1C, 0x1E, 0x0E, 0x12, 0x80, 0x02, 0x82, 0x82, 0x20, 0x21, 0x01, 0x21, 0x8C, 0x0C, 0x8C, 0x80, 0x18, 0x1B, 0x0B, 0x13, 0x5C, 0x1F, 0x4F, 0x53, 0x74, 0x37, 0x47, 0x73, 0x54, 0x14, 0x44, 0x50, 0xB0, 0x32, 0x82, 0xB2, 0x1C, 0x1D, 0x0D, 0x11, 0x24, 0x25, 0x05, 0x21, 0x4C, 0x0F, 0x4F, 0x43, 0x00, 0x00, 0x00, 0x00, 0x44, 0x06, 0x46, 0x42, 0xEC, 0x2D, 0xCD, 0xE1, 0x58, 0x18, 0x48, 0x50, 0x50, 0x12, 0x42, 0x52, 0xE8, 0x2B, 0xCB, 0xE3, 0x7C, 0x3E, 0x4E, 0x72, 0xD8, 0x1A, 0xCA, 0xD2, 0xC8, 0x09, 0xC9, 0xC1, 0xFC, 0x3D, 0xCD, 0xF1, 0x30, 0x30, 0x00, 0x30, 0x94, 0x15, 0x85, 0x91, 0x64, 0x25, 0x45, 0x61, 0x3C, 0x3C, 0x0C, 0x30, 0xB4, 0x36, 0x86, 0xB2, 0xE4, 0x24, 0xC4, 0xE0, 0xB8, 0x3B, 0x8B, 0xB3, 0x7C, 0x3C, 0x4C, 0x70, 0x0C, 0x0E, 0x0E, 0x02, 0x50, 0x10, 0x40, 0x50, 0x38, 0x39, 0x09, 0x31, 0x24, 0x26, 0x06, 0x22, 0x30, 0x32, 0x02, 0x32, 0x84, 0x04, 0x84, 0x80, 0x68, 0x29, 0x49, 0x61, 0x90, 0x13, 0x83, 0x93, 0x34, 0x37, 0x07, 0x33, 0xE4, 0x27, 0xC7, 0xE3, 0x24, 0x24, 0x04, 0x20, 0xA4, 0x24, 0x84, 0xA0, 0xC8, 0x0B, 0xCB, 0xC3, 0x50, 0x13, 0x43, 0x53, 0x08, 0x0A, 0x0A, 0x02, 0x84, 0x07, 0x87, 0x83, 0xD8, 0x19, 0xC9, 0xD1, 0x4C, 0x0C, 0x4C, 0x40, 0x80, 0x03, 0x83, 0x83, 0x8C, 0x0F, 0x8F, 0x83, 0xCC, 0x0E, 0xCE, 0xC2, 0x38, 0x3B, 0x0B, 0x33, 0x48, 0x0A, 0x4A, 0x42, 0xB4, 0x37, 0x87, 0xB3, 0xA1, 0xA8, 0x29, 0x89, 0x81, 0x84, 0x05, 0x85, 0xD2, 0xD4, 0x16, 0xC6, 0xD3, 0xD0, 0x13, 0xC3, 0x50, 0x54, 0x14, 0x44, 0x11, 0x1C, 0x1D, 0x0D, 0xA0, 0xAC, 0x2C, 0x8C, 0x21, 0x24, 0x25, 0x05, 0x51, 0x5C, 0x1D, 0x4D, 0x43, 0x40, 0x03, 0x43, 0x10, 0x18, 0x18, 0x08, 0x12, 0x1C, 0x1E, 0x0E, 0x51, 0x50, 0x11, 0x41, 0xF0, 0xFC, 0x3C, 0xCC, 0xC2, 0xC8, 0x0A, 0xCA, 0x63, 0x60, 0x23, 0x43, 0x20, 0x28, 0x28, 0x08, 0x40, 0x44, 0x04, 0x44, 0x20, 0x20, 0x20, 0x00, 0x91, 0x9C, 0x1D, 0x8D, 0xE0, 0xE0, 0x20, 0xC0, 0xE2, 0xE0, 0x22, 0xC2, 0xC0, 0xC8, 0x08, 0xC8, 0x13, 0x14, 0x17, 0x07, 0xA1, 0xA4, 0x25, 0x85, 0x83, 0x8C, 0x0F, 0x8F, 0x03, 0x00, 0x03, 0x03, 0x73, 0x78, 0x3B, 0x4B, 0xB3, 0xB8, 0x3B, 0x8B, 0x13, 0x10, 0x13, 0x03, 0xD2, 0xD0, 0x12, 0xC2, 0xE2, 0xEC, 0x2E, 0xCE, 0x70, 0x70, 0x30, 0x40, 0x80, 0x8C, 0x0C, 0x8C, 0x33, 0x3C, 0x3F, 0x0F, 0xA0, 0xA8, 0x28, 0x88, 0x32, 0x30, 0x32, 0x02, 0xD1, 0xDC, 0x1D, 0xCD, 0xF2, 0xF4, 0x36, 0xC6, 0x70, 0x74, 0x34, 0x44, 0xE0, 0xEC, 0x2C, 0xCC, 0x91, 0x94, 0x15, 0x85, 0x03, 0x08, 0x0B, 0x0B, 0x53, 0x54, 0x17, 0x47, 0x50, 0x5C, 0x1C, 0x4C, 0x53, 0x58, 0x1B, 0x4B, 0xB1, 0xBC, 0x3D, 0x8D, 0x01, 0x00, 0x01, 0x01, 0x20, 0x24, 0x24, 0x04, 0x10, 0x1C, 0x1C, 0x0C, 0x73, 0x70, 0x33, 0x43, 0x90, 0x98, 0x18, 0x88, 0x10, 0x10, 0x10, 0x00, 0xC0, 0xCC, 0x0C, 0xCC, 0xF2, 0xF0, 0x32, 0xC2, 0xD1, 0xD8, 0x19, 0xC9, 0x20, 0x2C, 0x2C, 0x0C, 0xE3, 0xE4, 0x27, 0xC7, 0x72, 0x70, 0x32, 0x42, 0x83, 0x80, 0x03, 0x83, 0x93, 0x98, 0x1B, 0x8B, 0xD1, 0xD0, 0x11, 0xC1, 0x82, 0x84, 0x06, 0x86, 0xC1, 0xC8, 0x09, 0xC9, 0x60, 0x60, 0x20, 0x40, 0x50, 0x50, 0x10, 0x40, 0xA3, 0xA0, 0x23, 0x83, 0xE3, 0xE8, 0x2B, 0xCB, 0x01, 0x0C, 0x0D, 0x0D, 0xB2, 0xB4, 0x36, 0x86, 0x92, 0x9C, 0x1E, 0x8E, 0x43, 0x4C, 0x0F, 0x4F, 0xB3, 0xB4, 0x37, 0x87, 0x52, 0x58, 0x1A, 0x4A, 0xC2, 0xC4, 0x06, 0xC6, 0x70, 0x78, 0x38, 0x48, 0xA2, 0xA4, 0x26, 0x86, 0x12, 0x10, 0x12, 0x02, 0xA3, 0xAC, 0x2F, 0x8F, 0xD1, 0xD4, 0x15, 0xC5, 0x61, 0x60, 0x21, 0x41, 0xC3, 0xC0, 0x03, 0xC3, 0xB0, 0xB4, 0x34, 0x84, 0x41, 0x40, 0x01, 0x41, 0x52, 0x50, 0x12, 0x42, 0x71, 0x7C, 0x3D, 0x4D, 0x81, 0x8C, 0x0D, 0x8D, 0x00, 0x08, 0x08, 0x08, 0x13, 0x1C, 0x1F, 0x0F, 0x91, 0x98, 0x19, 0x89, 0x00, 0x00, 0x00, 0x00, 0x11, 0x18, 0x19, 0x09, 0x00, 0x04, 0x04, 0x04, 0x53, 0x50, 0x13, 0x43, 0xF3, 0xF4, 0x37, 0xC7, 0xE1, 0xE0, 0x21, 0xC1, 0xF1, 0xFC, 0x3D, 0xCD, 0x72, 0x74, 0x36, 0x46, 0x23, 0x2C, 0x2F, 0x0F, 0x23, 0x24, 0x27, 0x07, 0xB0, 0xB0, 0x30, 0x80, 0x83, 0x88, 0x0B, 0x8B, 0x02, 0x0C, 0x0E, 0x0E, 0xA3, 0xA8, 0x2B, 0x8B, 0xA2, 0xA0, 0x22, 0x82, 0x62, 0x6C, 0x2E, 0x4E, 0x93, 0x90, 0x13, 0x83, 0x41, 0x4C, 0x0D, 0x4D, 0x61, 0x68, 0x29, 0x49, 0x70, 0x7C, 0x3C, 0x4C, 0x01, 0x08, 0x09, 0x09, 0x02, 0x08, 0x0A, 0x0A, 0xB3, 0xBC, 0x3F, 0x8F, 0xE3, 0xEC, 0x2F, 0xCF, 0xF3, 0xF0, 0x33, 0xC3, 0xC1, 0xC4, 0x05, 0xC5, 0x83, 0x84, 0x07, 0x87, 0x10, 0x14, 0x14, 0x04, 0xF2, 0xFC, 0x3E, 0xCE, 0x60, 0x64, 0x24, 0x44, 0xD2, 0xDC, 0x1E, 0xCE, 0x22, 0x2C, 0x2E, 0x0E, 0x43, 0x48, 0x0B, 0x4B, 0x12, 0x18, 0x1A, 0x0A, 0x02, 0x04, 0x06, 0x06, 0x21, 0x20, 0x21, 0x01, 0x63, 0x68, 0x2B, 0x4B, 0x62, 0x64, 0x26, 0x46, 0x02, 0x00, 0x02, 0x02, 0xF1, 0xF4, 0x35, 0xC5, 0x92, 0x90, 0x12, 0x82, 0x82, 0x88, 0x0A, 0x8A, 0x00, 0x0C, 0x0C, 0x0C, 0xB3, 0xB0, 0x33, 0x83, 0x72, 0x7C, 0x3E, 0x4E, 0xD0, 0xD0, 0x10, 0xC0, 0x72, 0x78, 0x3A, 0x4A, 0x43, 0x44, 0x07, 0x47, 0x92, 0x94, 0x16, 0x86, 0xE1, 0xE4, 0x25, 0xC5, 0x22, 0x24, 0x26, 0x06, 0x80, 0x80, 0x00, 0x80, 0xA1, 0xAC, 0x2D, 0x8D, 0xD3, 0xDC, 0x1F, 0xCF, 0xA1, 0xA0, 0x21, 0x81, 0x30, 0x30, 0x30, 0x00, 0x33, 0x34, 0x37, 0x07, 0xA2, 0xAC, 0x2E, 0x8E, 0x32, 0x34, 0x36, 0x06, 0x11, 0x14, 0x15, 0x05, 0x22, 0x20, 0x22, 0x02, 0x30, 0x38, 0x38, 0x08, 0xF0, 0xF4, 0x34, 0xC4, 0xA3, 0xA4, 0x27, 0x87, 0x41, 0x44, 0x05, 0x45, 0x40, 0x4C, 0x0C, 0x4C, 0x81, 0x80, 0x01, 0x81, 0xE1, 0xE8, 0x29, 0xC9, 0x80, 0x84, 0x04, 0x84, 0x93, 0x94, 0x17, 0x87, 0x31, 0x34, 0x35, 0x05, 0xC3, 0xC8, 0x0B, 0xCB, 0xC2, 0xCC, 0x0E, 0xCE, 0x30, 0x3C, 0x3C, 0x0C, 0x71, 0x70, 0x31, 0x41, 0x11, 0x10, 0x11, 0x01, 0xC3, 0xC4, 0x07, 0xC7, 0x81, 0x88, 0x09, 0x89, 0x71, 0x74, 0x35, 0x45, 0xF3, 0xF8, 0x3B, 0xCB, 0xD2, 0xD8, 0x1A, 0xCA, 0xF0, 0xF8, 0x38, 0xC8, 0x90, 0x94, 0x14, 0x84, 0x51, 0x58, 0x19, 0x49, 0x82, 0x80, 0x02, 0x82, 0xC0, 0xC4, 0x04, 0xC4, 0xF3, 0xFC, 0x3F, 0xCF, 0x41, 0x48, 0x09, 0x49, 0x31, 0x38, 0x39, 0x09, 0x63, 0x64, 0x27, 0x47, 0xC0, 0xC0, 0x00, 0xC0, 0xC3, 0xCC, 0x0F, 0xCF, 0xD3, 0xD4, 0x17, 0xC7, 0xB0, 0xB8, 0x38, 0x88, 0x03, 0x0C, 0x0F, 0x0F, 0x82, 0x8C, 0x0E, 0x8E, 0x42, 0x40, 0x02, 0x42, 0x23, 0x20, 0x23, 0x03, 0x91, 0x90, 0x11, 0x81, 0x60, 0x6C, 0x2C, 0x4C, 0xD3, 0xD8, 0x1B, 0xCB, 0xA0, 0xA4, 0x24, 0x84, 0x30, 0x34, 0x34, 0x04, 0xF1, 0xF0, 0x31, 0xC1, 0x40, 0x48, 0x08, 0x48, 0xC2, 0xC0, 0x02, 0xC2, 0x63, 0x6C, 0x2F, 0x4F, 0x31, 0x3C, 0x3D, 0x0D, 0x21, 0x2C, 0x2D, 0x0D, 0x40, 0x40, 0x00, 0x40, 0xB2, 0xBC, 0x3E, 0x8E, 0x32, 0x3C, 0x3E, 0x0E, 0xB0, 0xBC, 0x3C, 0x8C, 0xC1, 0xC0, 0x01, 0xC1, 0xA2, 0xA8, 0x2A, 0x8A, 0xB2, 0xB8, 0x3A, 0x8A, 0x42, 0x4C, 0x0E, 0x4E, 0x51, 0x54, 0x15, 0x45, 0x33, 0x38, 0x3B, 0x0B, 0xD0, 0xDC, 0x1C, 0xCC, 0x60, 0x68, 0x28, 0x48, 0x73, 0x7C, 0x3F, 0x4F, 0x90, 0x9C, 0x1C, 0x8C, 0xD0, 0xD8, 0x18, 0xC8, 0x42, 0x48, 0x0A, 0x4A, 0x52, 0x54, 0x16, 0x46, 0x73, 0x74, 0x37, 0x47, 0xA0, 0xA0, 0x20, 0x80, 0xE1, 0xEC, 0x2D, 0xCD, 0x42, 0x44, 0x06, 0x46, 0xB1, 0xB4, 0x35, 0x85, 0x23, 0x28, 0x2B, 0x0B, 0x61, 0x64, 0x25, 0x45, 0xF2, 0xF8, 0x3A, 0xCA, 0xE3, 0xE0, 0x23, 0xC3, 0xB1, 0xB8, 0x39, 0x89, 0xB1, 0xB0, 0x31, 0x81, 0x93, 0x9C, 0x1F, 0x8F, 0x52, 0x5C, 0x1E, 0x4E, 0xF1, 0xF8, 0x39, 0xC9, 0xE2, 0xE4, 0x26, 0xC6, 0xB2, 0xB0, 0x32, 0x82, 0x31, 0x30, 0x31, 0x01, 0xE2, 0xE8, 0x2A, 0xCA, 0x61, 0x6C, 0x2D, 0x4D, 0x53, 0x5C, 0x1F, 0x4F, 0xE0, 0xE4, 0x24, 0xC4, 0xF0, 0xF0, 0x30, 0xC0, 0xC1, 0xCC, 0x0D, 0xCD, 0x80, 0x88, 0x08, 0x88, 0x12, 0x14, 0x16, 0x06, 0x32, 0x38, 0x3A, 0x0A, 0x50, 0x58, 0x18, 0x48, 0xD0, 0xD4, 0x14, 0xC4, 0x62, 0x60, 0x22, 0x42, 0x21, 0x28, 0x29, 0x09, 0x03, 0x04, 0x07, 0x07, 0x33, 0x30, 0x33, 0x03, 0xE0, 0xE8, 0x28, 0xC8, 0x13, 0x18, 0x1B, 0x0B, 0x01, 0x04, 0x05, 0x05, 0x71, 0x78, 0x39, 0x49, 0x90, 0x90, 0x10, 0x80, 0x62, 0x68, 0x2A, 0x4A, 0x22, 0x28, 0x2A, 0x0A, 0x92, 0x98, 0x1A, 0x8A, 0x08, 0x30, 0x38, 0x38, 0xC8, 0xE0, 0xE8, 0x28, 0x0D, 0x21, 0x2C, 0x2D, 0x86, 0xA2, 0xA4, 0x26, 0xCF, 0xC3, 0xCC, 0x0F, 0xCE, 0xD2, 0xDC, 0x1E, 0x83, 0xB3, 0xB0, 0x33, 0x88, 0xB0, 0xB8, 0x38, 0x8F, 0xA3, 0xAC, 0x2F, 0x40, 0x60, 0x60, 0x20, 0x45, 0x51, 0x54, 0x15, 0xC7, 0xC3, 0xC4, 0x07, 0x44, 0x40, 0x44, 0x04, 0x4F, 0x63, 0x6C, 0x2F, 0x4B, 0x63, 0x68, 0x2B, 0x4B, 0x53, 0x58, 0x1B, 0xC3, 0xC3, 0xC0, 0x03, 0x42, 0x62, 0x60, 0x22, 0x03, 0x33, 0x30, 0x33, 0x85, 0xB1, 0xB4, 0x35, 0x09, 0x21, 0x28, 0x29, 0x80, 0xA0, 0xA0, 0x20, 0xC2, 0xE2, 0xE0, 0x22, 0x87, 0xA3, 0xA4, 0x27, 0xC3, 0xD3, 0xD0, 0x13, 0x81, 0x91, 0x90, 0x11, 0x01, 0x11, 0x10, 0x11, 0x06, 0x02, 0x04, 0x06, 0x0C, 0x10, 0x1C, 0x1C, 0x8C, 0xB0, 0xBC, 0x3C, 0x06, 0x32, 0x34, 0x36, 0x4B, 0x43, 0x48, 0x0B, 0xCF, 0xE3, 0xEC, 0x2F, 0x88, 0x80, 0x88, 0x08, 0x4C, 0x60, 0x6C, 0x2C, 0x88, 0xA0, 0xA8, 0x28, 0x07, 0x13, 0x14, 0x17, 0xC4, 0xC0, 0xC4, 0x04, 0x06, 0x12, 0x14, 0x16, 0xC4, 0xF0, 0xF4, 0x34, 0xC2, 0xC2, 0xC0, 0x02, 0x45, 0x41, 0x44, 0x05, 0xC1, 0xE1, 0xE0, 0x21, 0xC6, 0xD2, 0xD4, 0x16, 0x0F, 0x33, 0x3C, 0x3F, 0x0D, 0x31, 0x3C, 0x3D, 0x8E, 0x82, 0x8C, 0x0E, 0x88, 0x90, 0x98, 0x18, 0x08, 0x20, 0x28, 0x28, 0x4E, 0x42, 0x4C, 0x0E, 0xC6, 0xF2, 0xF4, 0x36, 0x0E, 0x32, 0x3C, 0x3E, 0x85, 0xA1, 0xA4, 0x25, 0xC9, 0xF1, 0xF8, 0x39, 0x0D, 0x01, 0x0C, 0x0D, 0xCF, 0xD3, 0xDC, 0x1F, 0xC8, 0xD0, 0xD8, 0x18, 0x0B, 0x23, 0x28, 0x2B, 0x46, 0x62, 0x64, 0x26, 0x4A, 0x72, 0x78, 0x3A, 0x07, 0x23, 0x24, 0x27, 0x0F, 0x23, 0x2C, 0x2F, 0xC1, 0xF1, 0xF0, 0x31, 0x42, 0x72, 0x70, 0x32, 0x42, 0x42, 0x40, 0x02, 0xC4, 0xD0, 0xD4, 0x14, 0x41, 0x41, 0x40, 0x01, 0xC0, 0xC0, 0xC0, 0x00, 0x43, 0x73, 0x70, 0x33, 0x47, 0x63, 0x64, 0x27, 0x8C, 0xA0, 0xAC, 0x2C, 0x8B, 0x83, 0x88, 0x0B, 0xC7, 0xF3, 0xF4, 0x37, 0x8D, 0xA1, 0xAC, 0x2D, 0x80, 0x80, 0x80, 0x00, 0x0F, 0x13, 0x1C, 0x1F, 0xCA, 0xC2, 0xC8, 0x0A, 0x0C, 0x20, 0x2C, 0x2C, 0x8A, 0xA2, 0xA8, 0x2A, 0x04, 0x30, 0x34, 0x34, 0xC2, 0xD2, 0xD0, 0x12, 0x0B, 0x03, 0x08, 0x0B, 0xCE, 0xE2, 0xEC, 0x2E, 0xC9, 0xE1, 0xE8, 0x29, 0x4D, 0x51, 0x5C, 0x1D, 0x84, 0x90, 0x94, 0x14, 0x08, 0x10, 0x18, 0x18, 0xC8, 0xF0, 0xF8, 0x38, 0x47, 0x53, 0x54, 0x17, 0x8E, 0xA2, 0xAC, 0x2E, 0x08, 0x00, 0x08, 0x08, 0xC5, 0xC1, 0xC4, 0x05, 0x03, 0x13, 0x10, 0x13, 0xCD, 0xC1, 0xCC, 0x0D, 0x86, 0x82, 0x84, 0x06, 0x89, 0xB1, 0xB8, 0x39, 0xCF, 0xF3, 0xFC, 0x3F, 0x4D, 0x71, 0x7C, 0x3D, 0xC1, 0xC1, 0xC0, 0x01, 0x01, 0x31, 0x30, 0x31, 0xC5, 0xF1, 0xF4, 0x35, 0x8A, 0x82, 0x88, 0x0A, 0x4A, 0x62, 0x68, 0x2A, 0x81, 0xB1, 0xB0, 0x31, 0xC1, 0xD1, 0xD0, 0x11, 0x00, 0x20, 0x20, 0x20, 0xC7, 0xD3, 0xD4, 0x17, 0x02, 0x02, 0x00, 0x02, 0x02, 0x22, 0x20, 0x22, 0x04, 0x00, 0x04, 0x04, 0x48, 0x60, 0x68, 0x28, 0x41, 0x71, 0x70, 0x31, 0x07, 0x03, 0x04, 0x07, 0xCB, 0xD3, 0xD8, 0x1B, 0x8D, 0x91, 0x9C, 0x1D, 0x89, 0x91, 0x98, 0x19, 0x41, 0x61, 0x60, 0x21, 0x8E, 0xB2, 0xBC, 0x3E, 0xC6, 0xE2, 0xE4, 0x26, 0x49, 0x51, 0x58, 0x19, 0xCD, 0xD1, 0xDC, 0x1D, 0x41, 0x51, 0x50, 0x11, 0x80, 0x90, 0x90, 0x10, 0xCC, 0xD0, 0xDC, 0x1C, 0x8A, 0x92, 0x98, 0x1A, 0x83, 0xA3, 0xA0, 0x23, 0x8B, 0xA3, 0xA8, 0x2B, 0xC0, 0xD0, 0xD0, 0x10, 0x81, 0x81, 0x80, 0x01, 0x0F, 0x03, 0x0C, 0x0F, 0x47, 0x43, 0x44, 0x07, 0x0A, 0x12, 0x18, 0x1A, 0xC3, 0xE3, 0xE0, 0x23, 0xCC, 0xE0, 0xEC, 0x2C, 0x8D, 0x81, 0x8C, 0x0D, 0x8F, 0xB3, 0xBC, 0x3F, 0x86, 0x92, 0x94, 0x16, 0x4B, 0x73, 0x78, 0x3B, 0x4C, 0x50, 0x5C, 0x1C, 0x82, 0xA2, 0xA0, 0x22, 0x81, 0xA1, 0xA0, 0x21, 0x43, 0x63, 0x60, 0x23, 0x03, 0x23, 0x20, 0x23, 0x4D, 0x41, 0x4C, 0x0D, 0xC8, 0xC0, 0xC8, 0x08, 0x8E, 0x92, 0x9C, 0x1E, 0x8C, 0x90, 0x9C, 0x1C, 0x0A, 0x32, 0x38, 0x3A, 0x0C, 0x00, 0x0C, 0x0C, 0x0E, 0x22, 0x2C, 0x2E, 0x8A, 0xB2, 0xB8, 0x3A, 0x4E, 0x62, 0x6C, 0x2E, 0x8F, 0x93, 0x9C, 0x1F, 0x4A, 0x52, 0x58, 0x1A, 0xC2, 0xF2, 0xF0, 0x32, 0x82, 0x92, 0x90, 0x12, 0xC3, 0xF3, 0xF0, 0x33, 0x49, 0x41, 0x48, 0x09, 0x48, 0x70, 0x78, 0x38, 0xCC, 0xC0, 0xCC, 0x0C, 0x05, 0x11, 0x14, 0x15, 0xCB, 0xF3, 0xF8, 0x3B, 0x40, 0x70, 0x70, 0x30, 0x45, 0x71, 0x74, 0x35, 0x4F, 0x73, 0x7C, 0x3F, 0x05, 0x31, 0x34, 0x35, 0x00, 0x10, 0x10, 0x10, 0x03, 0x03, 0x00, 0x03, 0x44, 0x60, 0x64, 0x24, 0x4D, 0x61, 0x6C, 0x2D, 0xC6, 0xC2, 0xC4, 0x06, 0x44, 0x70, 0x74, 0x34, 0xC5, 0xD1, 0xD4, 0x15, 0x84, 0xB0, 0xB4, 0x34, 0xCA, 0xE2, 0xE8, 0x2A, 0x09, 0x01, 0x08, 0x09, 0x46, 0x72, 0x74, 0x36, 0x09, 0x11, 0x18, 0x19, 0xCE, 0xF2, 0xFC, 0x3E, 0x40, 0x40, 0x40, 0x00, 0x02, 0x12, 0x10, 0x12, 0xC0, 0xE0, 0xE0, 0x20, 0x8D, 0xB1, 0xBC, 0x3D, 0x05, 0x01, 0x04, 0x05, 0xCA, 0xF2, 0xF8, 0x3A, 0x01, 0x01, 0x00, 0x01, 0xC0, 0xF0, 0xF0, 0x30, 0x0A, 0x22, 0x28, 0x2A, 0x4E, 0x52, 0x5C, 0x1E, 0x89, 0xA1, 0xA8, 0x29, 0x46, 0x52, 0x54, 0x16, 0x43, 0x43, 0x40, 0x03, 0x85, 0x81, 0x84, 0x05, 0x04, 0x10, 0x14, 0x14, 0x89, 0x81, 0x88, 0x09, 0x8B, 0x93, 0x98, 0x1B, 0x80, 0xB0, 0xB0, 0x30, 0xC5, 0xE1, 0xE4, 0x25, 0x48, 0x40, 0x48, 0x08, 0x49, 0x71, 0x78, 0x39, 0x87, 0x93, 0x94, 0x17, 0xCC, 0xF0, 0xFC, 0x3C, 0x0E, 0x12, 0x1C, 0x1E, 0x82, 0x82, 0x80, 0x02, 0x01, 0x21, 0x20, 0x21, 0x8C, 0x80, 0x8C, 0x0C, 0x0B, 0x13, 0x18, 0x1B, 0x4F, 0x53, 0x5C, 0x1F, 0x47, 0x73, 0x74, 0x37, 0x44, 0x50, 0x54, 0x14, 0x82, 0xB2, 0xB0, 0x32, 0x0D, 0x11, 0x1C, 0x1D, 0x05, 0x21, 0x24, 0x25, 0x4F, 0x43, 0x4C, 0x0F, 0x00, 0x00, 0x00, 0x00, 0x46, 0x42, 0x44, 0x06, 0xCD, 0xE1, 0xEC, 0x2D, 0x48, 0x50, 0x58, 0x18, 0x42, 0x52, 0x50, 0x12, 0xCB, 0xE3, 0xE8, 0x2B, 0x4E, 0x72, 0x7C, 0x3E, 0xCA, 0xD2, 0xD8, 0x1A, 0xC9, 0xC1, 0xC8, 0x09, 0xCD, 0xF1, 0xFC, 0x3D, 0x00, 0x30, 0x30, 0x30, 0x85, 0x91, 0x94, 0x15, 0x45, 0x61, 0x64, 0x25, 0x0C, 0x30, 0x3C, 0x3C, 0x86, 0xB2, 0xB4, 0x36, 0xC4, 0xE0, 0xE4, 0x24, 0x8B, 0xB3, 0xB8, 0x3B, 0x4C, 0x70, 0x7C, 0x3C, 0x0E, 0x02, 0x0C, 0x0E, 0x40, 0x50, 0x50, 0x10, 0x09, 0x31, 0x38, 0x39, 0x06, 0x22, 0x24, 0x26, 0x02, 0x32, 0x30, 0x32, 0x84, 0x80, 0x84, 0x04, 0x49, 0x61, 0x68, 0x29, 0x83, 0x93, 0x90, 0x13, 0x07, 0x33, 0x34, 0x37, 0xC7, 0xE3, 0xE4, 0x27, 0x04, 0x20, 0x24, 0x24, 0x84, 0xA0, 0xA4, 0x24, 0xCB, 0xC3, 0xC8, 0x0B, 0x43, 0x53, 0x50, 0x13, 0x0A, 0x02, 0x08, 0x0A, 0x87, 0x83, 0x84, 0x07, 0xC9, 0xD1, 0xD8, 0x19, 0x4C, 0x40, 0x4C, 0x0C, 0x83, 0x83, 0x80, 0x03, 0x8F, 0x83, 0x8C, 0x0F, 0xCE, 0xC2, 0xCC, 0x0E, 0x0B, 0x33, 0x38, 0x3B, 0x4A, 0x42, 0x48, 0x0A, 0x87, 0xB3, 0xB4, 0x37 };
            byte[] cp = new byte[32] { 0xD0, 0x73, 0x01, 0x5C, 0x3C, 0x75, 0x4B, 0xC2, 0xC1, 0x8C, 0xE9, 0x4A, 0xB0, 0xD1, 0x0F, 0x3E, 0x26, 0x8D, 0x66, 0xA7, 0x35, 0xA8, 0x1A, 0x81, 0x6F, 0xBA, 0xD9, 0xFA, 0x36, 0x16, 0x25, 0x01 };

            for (uint i = 0; i < 1024; i++)
                memory.Add(i, (byte)0x00);

            uint StartSC23seq = 0x02247E1C;
            uint SC23seqLen = 4096;

            for (uint i = StartSC23seq; i < (StartSC23seq + SC23seqLen); i++)
                memory.Add(i, sc23[i - StartSC23seq]);

            uint StartCPseq = 0x21018B0;
            uint CPseqLen = 32;

            for (uint i = StartCPseq; i < (StartCPseq + CPseqLen); i++)
                memory.Add(i, cp[i - StartCPseq]);

            BigEndian = true;

        }
        
        public SS02DynpassGen()
        {
            MemoryInit();
            #region Old
            /*
            StringBuilder sb = new StringBuilder();
            using (StreamWriter ser = new StreamWriter(File.Open("ser.txt", FileMode.OpenOrCreate)))
            {
                sb.Append("byte[] sc23 = new byte[4096]{");
                ker.Seek(0, SeekOrigin.Begin);
                sb.Append("0x" + ker.ReadByte().ToString("X2"));
                for (int i = 1; i < 4096; i++)
                {
                    sb.Append(", 0x"+ker.ReadByte().ToString("X2"));

                }
                sb.Append(" };");
                ser.WriteLine(sb.ToString());

                sb = new StringBuilder();

                sb.Append("byte[] cp = new byte[32]{");
                //ker.Seek(4096, SeekOrigin.Begin);
                sb.Append("0x" + ker.ReadByte().ToString("X2"));
                for (int i = 1; i < 32; i++)
                {
                    sb.Append(", 0x" + ker.ReadByte().ToString("X2"));

                }
                sb.Append(" };");
                ser.WriteLine(sb.ToString());
                
                
            }
            */

            

            /*
                byte[] sc23 = new byte[4096];
                byte[] cp = new byte[32];

                ker.Seek(StartSC23seq - StratKerInd, SeekOrigin.Begin);

                for (uint i = StartSC23seq; i < (StartSC23seq + SC23seqLen); i++)
                    memory.Add(i, (byte)ker.ReadByte());
                
                ker.Seek(StartSC23seq - StratKerInd, SeekOrigin.Begin);

                for (uint i = 0; i < 4096; i++)
                    sc23[i] = (byte)ker.ReadByte();
                

                uint StartCPseq = 0x21018B0;
                uint CPseqLen = 32;

                ker.Seek(StartCPseq - StratKerInd, SeekOrigin.Begin);

                for (uint i = StartCPseq; i < (StartCPseq + CPseqLen); i++)
                    memory.Add(i, (byte)ker.ReadByte());

                ker.Seek(StartCPseq - StratKerInd, SeekOrigin.Begin);

                for (uint i = 0; i < 32; i++)
                    cp[i] = (byte)ker.ReadByte();


                using (FileStream kryp = File.Open("crypdata.bin", FileMode.OpenOrCreate))
                {
                    kryp.Write(sc23, 0, 4096);
                    kryp.Write(cp, 0, 32);
                }
                */
            #endregion
        }

        public SS02DynpassGen(byte[] LCH_data)
        {
            MemoryInit();
            Generate(LCH_data);
        }

        public void Generate(byte[] LCH_data)
        {
            SP = 512;

            SP = SP - 0xA8;

            R0 = SP + 0xA8 - 0x24;//входной массив
            for (uint i = 0; i < 8; i++) memory[R0 + i] = LCH_data[i];

            byte[] secprt = new byte[8] { 0x53, 0x45, 0x43, 0x50, 0x52, 0x54, 0x00, 0x00 };
            for (uint i = 0; i < 8; i++) memory[R0 + 8 + i] = secprt[i];
            

            R0 = SP + 0xA8 - 0x34;
            for (uint i = 0; i < 16; i++) memory[R0 + i] = 0;

            R0 = SP + 0xA8 - 0x44;
            for (uint i = 0; i < 16; i++) memory[R0 + i] = 0x0F;

            R0 = SP + 0xA8 - 0x54;
            for (uint i = 0; i < 16; i++) memory[R0 + i] = 0xA5;

            R0 = SP + 0xA8 - 0x64;
            for (uint i = 0; i < 16; i++) memory[R0 + i] = 0xF0;

            R0 = SP + 0xA8 - 0x74;
            for (uint i = 0; i < 16; i++) memory[R0 + i] = 0;

            R0 = SP + 0xA8 - 0x84;
            for (uint i = 0; i < 16; i++) memory[R0 + i] = 0;


            #region Prepare sequense

            R0 = 0;

            //ROM:020383A0         loc_20383A0                             ; CODE XREF: sub_20382A8+12Cj
        loc_20383A0:
            //ROM:020383A0                         ADD     R1, SP, #0xA8+var_24
            R1 = SP + 0xA8 - 0x24;
            //ROM:020383A4 4                       LDRB    R1, [R1,R0]
            R1 = get_byte((R1 + R0));
            //ROM:020383A8 4                       LDRB    R2, [SP,#0xA8+var_16]
            R2 = get_byte((SP + 0xA8 - 0x16));
            //ROM:020383AC 4                       EOR     R1, R2, R1
            R1 = R2 ^ R1;
            //ROM:020383B0 4                       STRB    R1, [SP,#0xA8+var_16]
            put_byte((SP + 0xA8 - 0x16), R1);
            //ROM:020383B4 4                       ADD     R1, SP, #0xA8+var_24
            R1 = SP + 0xA8 - 0x24;
            //ROM:020383B8 4                       LDRB    R1, [R1,R0,LSL#1]
            R1 = get_byte((R1 + (R0 << 1)));
            //ROM:020383BC 4                       LDRB    R2, [SP,#0xA8+var_15]
            R2 = get_byte((SP + 0xA8 - 0x15));
            //ROM:020383C0 4                       ADD     R0, R0, #1
            R0 = R0 + 1;
            //ROM:020383C4 4                       AND     R0, R0, #0xFF
            R0 = R0 & 0xFF;
            //ROM:020383C8 4                       EOR     R1, R2, R1
            R1 = R2 ^ R1;
            //ROM:020383CC 4                       STRB    R1, [SP,#0xA8+var_15]
            put_byte((SP + 0xA8 - 0x15), R1);
            //ROM:020383D0 4                       CMP     R0, #8
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(8)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(8)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(8)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 8) == 0 ? true : false;
            N = ((Int32)(R0 - 8)) < 0 ? true : false;
            //ROM:020383D4 4                       BLT     loc_20383A0
            if (N != V) goto loc_20383A0;
            //

            #endregion

            //ROM:020383D8 4                       ADD     R1, SP, #0xA8+var_34
            R1 = SP + 0xA8 - 0x34;
            //ROM:020383DC 4                       ADD     R0, SP, #0xA8+var_24
            R0 = SP + 0xA8 - 0x24;
            //ROM:020383E0 4                       MOV     R2, #0x10
            R2 = 0x10;
            //
            CryptuetPaket();


            //ROM:020383E8 4                       ADD     R2, SP, #0xA8+var_98
            R2 = SP + 0xA8 - 0x98;
            //ROM:020383EC 4                       ADD     R0, SP, #0xA8+var_34
            R0 = SP + 0xA8 - 0x34;
            //ROM:020383F0 4                       MOV     R1, #0x10
            R1 = 0x10;
            //ROM:020383F4 4                       BL      aTozheChegotoKriptuet ; R0 InBuf
            aTozheChegotoKriptuet();
            //ROM:020383F4                                                 ; R1 Len
            //ROM:020383F4                                                 ; R2 OutBuf
            //ROM:020383F8 4                       MOV     R5, #0xFF
            R5 = 0xFF;
            //ROM:020383FC 4                       ADD     R0, SP, #0xA8+var_43
            R0 = SP + 0xA8 - 0x43;
            //ROM:02038400 4                       MOV     R7, R0
            R7 = R0;
            //ROM:02038404 4                       STRB    R5, [SP,#0xA8+var_44]
            put_byte((SP + 0xA8 - 0x44), R5);
            //ROM:02038408 4                       ADD     R1, SP, #0xA8+var_98
            R1 = SP + 0xA8 - 0x98;
            //ROM:0203840C 4                       MOV     R2, #7
            R2 = 7;
            //ROM:02038410 4                       BL      aMemCpy
            aMemCpy();
            //ROM:02038414 4                       STRB    R5, [SP,#0xA8+var_40]
            put_byte((SP + 0xA8 - 0x40), R5);
            //ROM:02038418 4                       ADD     R1, SP, #0xA8+var_91
            R1 = SP + 0xA8 - 0x91;
            //ROM:0203841C 4                       ADD     R0, SP, #0xA8+var_54
            R0 = SP + 0xA8 - 0x54;
            //ROM:02038420 4                       MOV     R2, #8
            R2 = 8;
            //ROM:02038424 4                       BL      aMemCpy
            aMemCpy();
            //ROM:02038428 4                       STRB    R5, [SP,#0xA8+var_54]
            put_byte((SP + 0xA8 - 0x54), R5);
            //ROM:0203842C 4                       STRB    R5, [SP,#0xA8+var_50]
            put_byte((SP + 0xA8 - 0x50), R5);
            //ROM:02038430 4                       MOV     R1, SP
            R1 = SP;
            //ROM:02038434 4                       ADD     R0, SP, #0xA8+var_98
            R0 = SP + 0xA8 - 0x98;
            //ROM:02038438 4                       MOV     R2, #0x10
            R2 = 0x10;
            //ROM:0203843C 4                       BL      aCryptuetPaket  ; R0 InBuf
            CryptuetPaket();
            //ROM:0203843C                                                 ; R1 OutBuf
            //ROM:0203843C                                                 ; R2 Len
            //ROM:02038440 4                       MOV     R1, SP
            R1 = SP;
            //ROM:02038444 4                       LDMIA   R1, {R2,R3}
            R2 = get_long(R1 + 0x0);
            R3 = get_long(R1 + 0x4);
            //ROM:02038448 4                       ADD     R0, SP, #0xA8+var_74
            R0 = SP + 0xA8 - 0x74;
            //ROM:0203844C 4                       STMIA   R0, {R2,R3}
            put_long(R0 + 0x0, R2);
            put_long(R0 + 0x4, R3);
            //ROM:02038450 4                       MOV     R2, #8
            R2 = 8;
            //ROM:02038454 4                       ADD     R0, SP, #0xA8+var_84
            R0 = SP + 0xA8 - 0x84;
            //ROM:02038458 4                       ADD     R1, SP, #0xA8+var_A0
            R1 = SP + 0xA8 - 0xA0;
            //ROM:0203845C 4                       BL      aMemCpy
            aMemCpy();
            //ROM:02038460 4                       ADD     R2, SP, #0xA8+var_98
            R2 = SP + 0xA8 - 0x98;
            //ROM:02038464 4                       MOV     R0, SP
            R0 = SP;
            //ROM:02038468 4                       MOV     R1, #0x10
            R1 = 0x10;
            //ROM:0203846C 4                       BL      aTozheChegotoKriptuet ; R0 InBuf
            aTozheChegotoKriptuet();
            //ROM:0203846C                                                 ; R1 Len
            //ROM:0203846C                                                 ; R2 OutBuf
            //ROM:02038470 4                       ADD     R1, SP, #0xA8+var_98
            R1 = SP + 0xA8 - 0x98;
            //ROM:02038474 4                       LDMIA   R1, {R2,R3}
            R2 = get_long(R1 + 0x0);
            R3 = get_long(R1 + 0x4);
            //ROM:02038478 4                       ADD     R0, SP, #0xA8+var_64
            R0 = SP + 0xA8 - 0x64;
            //ROM:0203847C 4                       STMIA   R0, {R2,R3}
            put_long(R0 + 0x0, R2);
            put_long(R0 + 0x4, R3);
            //ROM:02038480 4                       STRB    R5, [SP,#0xA8+var_64]
            put_byte((SP + 0xA8 - 0x64), R5);
            //
            
            
            
            
            
            
            
            R0 = SP + 0xA8 - 0x84;//выходной массив
            for(uint i=0;i<8;i++)_secretSeed[i]= memory[R0+i];

            R0 = SP + 0xA8 - 0x54;//выходной массив
            for (uint i = 0; i < 3; i++) _writePass[i] = memory[R0+i+1];
        }

        private void CryptuetPaket()
        {
            //ROM:01DBA4A4 4                       STMFD   SP!, {R4,R5,LR}
            put_long(SP - 0x4, LR);
            put_long(SP - 0x8, R5);
            put_long(SP - 0xC, R4);
            SP = SP - 0xC;
            //ROM:01DBA4A8 4                       SUB     SP, SP, #0xA0
            SP = SP - 0xA0;
            //ROM:01DBA4AC 4                       MOV     R4, R1
            R4 = R1;
            //ROM:01DBA4B0 4                       LDR     R1, =unk_21018B0
            uint addr = 0x21018B0;
            //ROM:01DBA4B4 4                       MOV     R5, R0
            R5 = R0;
            //ROM:01DBA4B8 4                       LDMIA   R1, {R1,R3,R12,LR}
            R1 = get_long(addr + 0x0);
            R3 = get_long(addr + 0x4);
            R12 = get_long(addr + 0x8);
            LR = get_long(addr + 0xC);
            //ROM:01DBA4BC 4                       ADD     R0, SP, #0xAC+var_9C
            R0 = SP + 0xAC - 0x9C;
            //ROM:01DBA4C0 4                       STMIA   R0, {R1,R3,R12,LR}
            put_long(R0 + 0x0, R1);
            put_long(R0 + 0x4, R3);
            put_long(R0 + 0x8, R12);
            put_long(R0 + 0xC, LR);
            //ROM:01DBA4C4 4                       LDR     R1, =unk_21018C0
            addr = 0x21018C0;
            //ROM:01DBA4C8 4                       MOV     R0, SP
            R0 = SP;
            //ROM:01DBA4CC 4                       LDMIA   R1, {R1,R3,R12,LR}
            R1 = get_long(addr + 0x0);
            R3 = get_long(addr + 0x4);
            R12 = get_long(addr + 0x8);
            LR = get_long(addr + 0xC);
            //ROM:01DBA4D0 4                       STMIA   R0, {R1,R3,R12,LR}
            put_long(R0 + 0x0, R1);
            put_long(R0 + 0x4, R3);
            put_long(R0 + 0x8, R12);
            put_long(R0 + 0xC, LR);
            //ROM:01DBA4D4 4                       ADD     R1, SP, #0xAC+var_8C
            R1 = SP + 0xAC - 0x8C;
            //ROM:01DBA4D8 4                       ADD     R0, SP, #0xAC+var_9C
            R0 = SP + 0xAC - 0x9C;
            //ROM:01DBA4DC 4                       BL      Subcryptography_2
            Subcryptography_2();
            //ROM:01DBA4E0 4                       ADD     R2, SP, #0xAC+var_8C
            R2 = SP + 0xAC - 0x8C;
            //ROM:01DBA4E4 4                       MOV     R1, R4
            R1 = R4;
            //ROM:01DBA4E8 4                       MOV     R0, R5
            R0 = R5;
            //ROM:01DBA4EC 4                       BL      Subcryptography_3
            Subcryptography_3();
            //ROM:01DBA4F0 4                       ADD     SP, SP, #0xA0
            SP = SP + 0xA0;
            //ROM:01DBA4F4 4                       LDMFD   SP!, {R4,R5,PC}
            R4 = get_long(SP + 0x0);
            R5 = get_long(SP + 0x4);
            PC = get_long(SP + 0x8);
            SP = SP + 0xC;

            //


        }

        private void aTozheChegotoKriptuet()
        {
            //ROM:01BA53AC 4                       STMFD   SP!, {R4-R7,LR}
            put_long(SP - 0x4, LR);
            put_long(SP - 0x8, R7);
            put_long(SP - 0xC, R6);
            put_long(SP - 0x10, R5);
            put_long(SP - 0x14, R4);
            SP = SP - 0x14;
            //ROM:01BA53B0 4                       SUB     SP, SP, #0x64   ; âûäåëåíèå ìåñòà â ñòåêå ïîä ïåðåìåííûå, ïîä ñîõðàíåíèå ðåãèñòðîâ óæå âûäåëåíî 0õ14
            SP = SP - 0x64;
            //ROM:01BA53B4 4                       MOV     R6, R0          ; R6 óêàçàòåëü íà âõîäíîé áóôåð
            R6 = R0;
            //ROM:01BA53B8 4                       MOV     R5, R1          ; R5 äëèíà âõîäíîãî áóôåðà
            R5 = R1;
            //ROM:01BA53BC 4                       MOV     R1, #0x14
            R1 = 0x14;
            //ROM:01BA53C0 4                       ADR     R0, unk_1BA541C
            R0 = 0x1BA541C;
            //ROM:01BA53C4 4                       MOV     R4, R2          ; R4 óêàçàòåëü íà âûõîäíîé áóôåð
            R4 = R2;
            //ROM:01BA53C8 4                       BL      SistemPlanirov  ; R0 èìÿ çàäàíèÿ
            SistemPlanirov();
            //ROM:01BA53C8                                                 ; R1 êîìàíäà èëè ðàçìåð
            //ROM:01BA53CC 4                       MOV     R0, SP
            R0 = SP;
            //ROM:01BA53D0 4                       BL      GotovitKoefficienti ; R0 óêàçàòåëü íà áóôåð äëÿ êîýôôèöåíòîâ
            GotovitKoefficienti();
            //ROM:01BA53D4 4                       MOV     R7, #1
            R7 = 1;
            //ROM:01BA53D8 4                       CMP     R0, #0
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0) == 0 ? true : false;
            N = ((Int32)(R0 - 0)) < 0 ? true : false;
            //ROM:01BA53DC 4                       BNE     loc_1BA53F8
            if (!Z) goto loc_1BA53F8;
            //ROM:01BA53E0 4                       MOV     R2, R5
            R2 = R5;
            //ROM:01BA53E4 4                       MOV     R1, R6
            R1 = R6;
            //ROM:01BA53E8 4                       MOV     R0, SP
            R0 = SP;
            //ROM:01BA53EC 4                       BL      TCK_Gryptofunc_1 ; R0 Áóôåð êîýôôèöèåíòîâ
            TCK_Gryptofunc_1();
            //ROM:01BA53EC                                                 ; R1 Âõîäíîé áóôåð
            //ROM:01BA53EC                                                 ; R2 Äëèíà âõîäíîãî áóôåðà
            //ROM:01BA53F0 4                       CMP     R0, #0
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0) == 0 ? true : false;
            N = ((Int32)(R0 - 0)) < 0 ? true : false;
            //ROM:01BA53F4 4                       BEQ     loc_1BA5400
            if (Z) goto loc_1BA5400;
        //ROM:01BA53F8
        //ROM:01BA53F8         loc_1BA53F8                             ; CODE XREF: aTozheChegotoKriptuet+30j
        loc_1BA53F8:
            //ROM:01BA53F8 4                       MOV     R0, R7
            R0 = R7;
            //ROM:01BA53FC 4                       B       loc_1BA5414
            goto loc_1BA5414;
        //ROM:01BA5400         ; ---------------------------------------------------------------------------
        //ROM:01BA5400
        //ROM:01BA5400         loc_1BA5400                             ; CODE XREF: aTozheChegotoKriptuet+48j
        loc_1BA5400:
            //ROM:01BA5400 4                       MOV     R1, R4
            R1 = R4;
            //ROM:01BA5404 4                       MOV     R0, SP
            R0 = SP;
            //ROM:01BA5408 4                       BL      TCK_Gryptofunc_2 ; R0 óêàçàòåëü íà áóôåð êîýôôèöèåíòîâ
            TCK_Gryptofunc_2();
            //ROM:01BA5408                                                 ; R1 óêàçàòåëü íà âûõîäíîé áóôåð
            //ROM:01BA540C 4                       CMP     R0, #0
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0) == 0 ? true : false;
            N = ((Int32)(R0 - 0)) < 0 ? true : false;
            //ROM:01BA5410 4                       MOVNE   R0, #2
            if (!Z) R0 = 2;
        //ROM:01BA5414
        //ROM:01BA5414         loc_1BA5414                             ; CODE XREF: aTozheChegotoKriptuet+50j
        loc_1BA5414:
            //ROM:01BA5414 4                       ADD     SP, SP, #0x64
            SP = SP + 0x64;
            //ROM:01BA5418 4                       LDMFD   SP!, {R4-R7,PC}
            R4 = get_long(SP + 0x0);
            R5 = get_long(SP + 0x4);
            R6 = get_long(SP + 0x8);
            R7 = get_long(SP + 0xC);
            PC = get_long(SP + 0x10);
            SP = SP + 0x14;

            //

        }

        private void TCK_Gryptofunc_2()
        {
            //ROM:01BA55BC 4                       STMFD   SP!, {R4-R6,LR}
            put_long(SP - 0x4, LR);
            put_long(SP - 0x8, R6);
            put_long(SP - 0xC, R5);
            put_long(SP - 0x10, R4);
            SP = SP - 0x10;
            //ROM:01BA55C0 4                       MOV     R4, R0          ; R4 áóôôåð êîýôôèöèåíòîâ
            R4 = R0;
            //ROM:01BA55C4 4                       MOV     R5, R1          ; R5 âûõîäíîé áóôôåð
            R5 = R1;
            //ROM:01BA55C8 4                       MOV     R1, #0x8A
            R1 = 0x8A;
            //ROM:01BA55CC 4                       ADR     R0, unk_1BA541C
            R0 = 0x1BA541C;
            //ROM:01BA55D0 4                       BL      SistemPlanirov  ; R0 èìÿ çàäàíèÿ
            SistemPlanirov();
            //ROM:01BA55D0                                                 ; R1 êîìàíäà èëè ðàçìåð
            //ROM:01BA55D4 4                       MOV     R0, R4
            R0 = R4;
            //ROM:01BA55D8 4                       BL      TCK_G2_Subfunc  ; R0 áóôôåð êîýôôèöèåíòîâ
            TCK_G2_Subfunc();
            //ROM:01BA55DC 4                       MOV     R0, #0
            R0 = 0;
            //ROM:01BA55E0 4                       MOV     R6, #0
            R6 = 0;
        //ROM:01BA55E4
        //ROM:01BA55E4         loc_1BA55E4                             ; CODE XREF: TCK_Gryptofunc_2+4Cj
        loc_1BA55E4:
            //ROM:01BA55E4 4                       STRB    R6, [R4,R0]
            put_byte((R4 + R0), R6);
            //ROM:01BA55E8 4                       ADD     R0, R0, #1
            R0 = R0 + 1;
            //ROM:01BA55EC 4                       AND     R0, R0, #0xFF
            R0 = R0 & 0xFF;
            //ROM:01BA55F0 4                       CMP     R0, #0x40
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0x40)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0x40)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0x40)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0x40) == 0 ? true : false;
            N = ((Int32)(R0 - 0x40)) < 0 ? true : false;
            //ROM:01BA55F4 4                       BLT     loc_1BA55E4
            if (N != V) goto loc_1BA55E4;
            //ROM:01BA55F8 4                       STR     R6, [R4,#0x54]
            put_long((R4 + 0x54), R6);
            //ROM:01BA55FC 4                       STR     R6, [R4,#0x58]
            put_long((R4 + 0x58), R6);
            //ROM:01BA5600 4                       MOV     R0, #0
            R0 = 0;
            //ROM:01BA5604 4                       MOV     R1, #3
            R1 = 3;
        //ROM:01BA5608
        //ROM:01BA5608         loc_1BA5608                             ; CODE XREF: TCK_Gryptofunc_2+8Cj
        loc_1BA5608:
            //ROM:01BA5608 4                       MOV     R2, R0,ASR#2
            R2 = (UInt32)((Int32)R0 >> 2);
            //ROM:01BA560C 4                       ADD     R2, R4, R2,LSL#2
            R2 = R4 + (R2 << 2);
            //ROM:01BA5610 4                       LDR     R3, [R2,#0x40]
            R3 = get_long((R2 + 0x40));
            //ROM:01BA5614 4                       AND     R2, R0, #3
            R2 = R0 & 3;
            //ROM:01BA5618 4                       SUB     R2, R1, R2
            R2 = R1 - R2;
            //ROM:01BA561C 4                       MOV     R2, R2,LSL#3
            R2 = (R2 << 3);
            //ROM:01BA5620 4                       MOV     R2, R3,LSR R2
            R2 = (R3 >> (byte)R2);
            //ROM:01BA5624 4                       STRB    R2, [R5,R0]
            put_byte((R5 + R0), R2);
            //ROM:01BA5628 4                       ADD     R0, R0, #1
            R0 = R0 + 1;
            //ROM:01BA562C 4                       AND     R0, R0, #0xFF
            R0 = R0 & 0xFF;
            //ROM:01BA5630 4                       CMP     R0, #0x14
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0x14)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0x14)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0x14)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0x14) == 0 ? true : false;
            N = ((Int32)(R0 - 0x14)) < 0 ? true : false;
            //ROM:01BA5634 4                       BLT     loc_1BA5608
            if (N != V) goto loc_1BA5608;
            //ROM:01BA5638 4                       ADR     R0, unk_1BA541C
            R0 = 0x1BA541C;
            //ROM:01BA563C 4                       MOV     R1, #0x96
            R1 = 0x96;
            //ROM:01BA5640 4                       BL      SistemPlanirov  ; R0 èìÿ çàäàíèÿ
            SistemPlanirov();
            //ROM:01BA5640                                                 ; R1 êîìàíäà èëè ðàçìåð
            //ROM:01BA5644 4                       MOV     R0, R6
            R0 = R6;
            //ROM:01BA5648 4                       LDMFD   SP!, {R4-R6,PC}
            R4 = get_long(SP + 0x0);
            R5 = get_long(SP + 0x4);
            R6 = get_long(SP + 0x8);
            PC = get_long(SP + 0xC);
            SP = SP + 0x10;

            //
        }

        private void TCK_G2_Subfunc()
        {
            //ROM:01BA564C 4                       STMFD   SP!, {R4-R6,LR}
            put_long(SP - 0x4, LR);
            put_long(SP - 0x8, R6);
            put_long(SP - 0xC, R5);
            put_long(SP - 0x10, R4);
            SP = SP - 0x10;
            //ROM:01BA5650 4                       MOV     R4, R0          ; R4 áóôôåð êîýôôèöèåíòîâ
            R4 = R0;
            //ROM:01BA5654 4                       ADR     R0, unk_1BA541C
            R0 = 0x1BA541C;
            //ROM:01BA5658 4                       MOV     R1, #0xA5
            R1 = 0xA5;
            //ROM:01BA565C 4                       BL      SistemPlanirov  ; R0 èìÿ çàäàíèÿ
            SistemPlanirov();
            //ROM:01BA565C                                                 ; R1 êîìàíäà èëè ðàçìåð
            //ROM:01BA5660 4                       LDR     R0, [R4,#0x5C]  ; ñ÷èòûâàåì ðàçìåð áëîêà äàííûõ â áóôåðå
            R0 = get_long((R4 + 0x5C));
            //ROM:01BA5664 4                       MOV     R1, #0x80
            R1 = 0x80;
            //ROM:01BA5668 4                       ADD     R2, R0, #1
            R2 = R0 + 1;
            //ROM:01BA566C 4                       STR     R2, [R4,#0x5C]  ; óâåë÷èâàåì ðàçìåð áëîêà äàííûõ íà 1 è ñîõðàíÿåì
            put_long((R4 + 0x5C), R2);
            //ROM:01BA5670 4                       STRB    R1, [R4,R0]     ; çàïèñûâàåì 0õ80 â êîíåö áëîêà äàííûõ â áóôôåðå
            put_byte((R4 + R0), R1);
            //ROM:01BA5674 4                       ADD     R5, R4, #0x40   ; R5 óêàçàòåëü íà íà÷àëî áëîêà êîýôôèöèåíòîâ â áóôåðå
            R5 = R4 + 0x40;
            //ROM:01BA5678 4                       MOV     R6, #0
            R6 = 0;
            //ROM:01BA567C 4                       CMP     R0, #0x37       ; ñðàâíèâàåì ðàçìåð áëîêà äàííûõ ñ 0õ37
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0x37)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0x37)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0x37)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0x37) == 0 ? true : false;
            N = ((Int32)(R0 - 0x37)) < 0 ? true : false;
            //ROM:01BA5680 4                       BHI     loc_1BA5694
            if (C && !Z) goto loc_1BA5694;
            //ROM:01BA5684 4                       B       loc_1BA56DC     ; ñþäà åñëè ðàçìåð áä <= 37
            goto loc_1BA56DC;
        //ROM:01BA5688         ; ---------------------------------------------------------------------------
        //ROM:01BA5688
        //ROM:01BA5688         loc_1BA5688                             ; CODE XREF: TCK_G2_Subfunc+50j
        loc_1BA5688:
            //ROM:01BA5688 4                       ADD     R1, R0, #1
            R1 = R0 + 1;
            //ROM:01BA568C 4                       STR     R1, [R4,#0x5C]
            put_long((R4 + 0x5C), R1);
            //ROM:01BA5690 4                       STRB    R6, [R4,R0]
            put_byte((R4 + R0), R6);
        //ROM:01BA5694
        //ROM:01BA5694         loc_1BA5694                             ; CODE XREF: TCK_G2_Subfunc+34j
        loc_1BA5694:
            //ROM:01BA5694 4                       LDR     R0, [R4,#0x5C]
            R0 = get_long((R4 + 0x5C));
            //ROM:01BA5698 4                       CMP     R0, #0x40
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0x40)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0x40)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0x40)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0x40) == 0 ? true : false;
            N = ((Int32)(R0 - 0x40)) < 0 ? true : false;
            //ROM:01BA569C 4                       BCC     loc_1BA5688
            if (!C) goto loc_1BA5688;
            //ROM:01BA56A0 4                       MOV     R1, R5
            R1 = R5;
            //ROM:01BA56A4 4                       MOV     R0, R4
            R0 = R4;
            //ROM:01BA56A8 4                       BL      Subcryptography_1 ; R0 óêàçàòåëü íà áëîê äàííûõ
            Subcryptography_1();
            //ROM:01BA56A8                                                 ; R1 óêàçàòåëü íà áëîê êîýôôèöèåíòîâ
            //ROM:01BA56AC 4                       STR     R6, [R4,#0x5C]
            put_long((R4 + 0x5C), R6);
        //ROM:01BA56B0
        //ROM:01BA56B0         loc_1BA56B0                             ; CODE XREF: TCK_G2_Subfunc+7Cj
        loc_1BA56B0:
            //ROM:01BA56B0 4                       LDR     R0, [R4,#0x5C]
            R0 = get_long((R4 + 0x5C));
            //ROM:01BA56B4 4                       ADD     R1, R0, #1
            R1 = R0 + 1;
            //ROM:01BA56B8 4                       STR     R1, [R4,#0x5C]
            put_long((R4 + 0x5C), R1);
            //ROM:01BA56BC 4                       STRB    R6, [R4,R0]
            put_byte((R4 + R0), R6);
            //ROM:01BA56C0 4                       LDR     R0, [R4,#0x5C]
            R0 = get_long((R4 + 0x5C));
            //ROM:01BA56C4 4                       CMP     R0, #0x38
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0x38)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0x38)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0x38)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0x38) == 0 ? true : false;
            N = ((Int32)(R0 - 0x38)) < 0 ? true : false;
            //ROM:01BA56C8 4                       BCC     loc_1BA56B0
            if (!C) goto loc_1BA56B0;
            //ROM:01BA56CC 4                       B       loc_1BA56E8
            goto loc_1BA56E8;
        //ROM:01BA56D0         ; ---------------------------------------------------------------------------
        //ROM:01BA56D0
        //ROM:01BA56D0         loc_1BA56D0                             ; CODE XREF: TCK_G2_Subfunc+98j
        loc_1BA56D0:
            //ROM:01BA56D0 4                       ADD     R1, R0, #1      ; ñþäà åñëè áä < 0õ38
            R1 = R0 + 1;
            //ROM:01BA56D4 4                       STR     R1, [R4,#0x5C]
            put_long((R4 + 0x5C), R1);
            //ROM:01BA56D8 4                       STRB    R6, [R4,R0]
            put_byte((R4 + R0), R6);
        //ROM:01BA56DC
        //ROM:01BA56DC         loc_1BA56DC                             ; CODE XREF: TCK_G2_Subfunc+38j
        loc_1BA56DC:
            //ROM:01BA56DC 4                       LDR     R0, [R4,#0x5C]
            R0 = get_long((R4 + 0x5C));
            //ROM:01BA56E0 4                       CMP     R0, #0x38       ; ñðàâíèâàåì îáíîâë¸ííûé ðàçìåð áä ñ 0õ38
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0x38)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0x38)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0x38)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0x38) == 0 ? true : false;
            N = ((Int32)(R0 - 0x38)) < 0 ? true : false;
            //ROM:01BA56E4 4                       BCC     loc_1BA56D0     ; ñþäà åñëè áä < 0õ38
            if (!C) goto loc_1BA56D0;
        //ROM:01BA56E8
        //ROM:01BA56E8         loc_1BA56E8                             ; CODE XREF: TCK_G2_Subfunc+80j
        loc_1BA56E8:
            //ROM:01BA56E8 4                       ADR     R0, unk_1BA541C
            R0 = 0x1BA541C;
            //ROM:01BA56EC 4                       MOV     R1, #0xC0
            R1 = 0xC0;
            //ROM:01BA56F0 4                       BL      SistemPlanirov  ; R0 èìÿ çàäàíèÿ
            SistemPlanirov();
            //ROM:01BA56F0                                                 ; R1 êîìàíäà èëè ðàçìåð
            //ROM:01BA56F4 4                       LDR     R0, [R4,#0x58]
            R0 = get_long((R4 + 0x58));
            //ROM:01BA56F8 4                       MOV     R1, R0,LSR#24
            R1 = (R0 >> 24);
            //ROM:01BA56FC 4                       STRB    R1, [R4,#0x38]
            put_byte((R4 + 0x38), R1);
            //ROM:01BA5700 4                       MOV     R1, R0,LSR#16
            R1 = (R0 >> 16);
            //ROM:01BA5704 4                       STRB    R1, [R4,#0x39]
            put_byte((R4 + 0x39), R1);
            //ROM:01BA5708 4                       MOV     R1, R0,LSR#8
            R1 = (R0 >> 8);
            //ROM:01BA570C 4                       STRB    R1, [R4,#0x3A]
            put_byte((R4 + 0x3A), R1);
            //ROM:01BA5710 4                       STRB    R0, [R4,#0x3B]
            put_byte((R4 + 0x3B), R0);
            //ROM:01BA5714 4                       LDR     R0, [R4,#0x54]
            R0 = get_long((R4 + 0x54));
            //ROM:01BA5718 4                       MOV     R1, R0,LSR#24
            R1 = (R0 >> 24);
            //ROM:01BA571C 4                       STRB    R1, [R4,#0x3C]
            put_byte((R4 + 0x3C), R1);
            //ROM:01BA5720 4                       MOV     R1, R0,LSR#16
            R1 = (R0 >> 16);
            //ROM:01BA5724 4                       STRB    R1, [R4,#0x3D]
            put_byte((R4 + 0x3D), R1);
            //ROM:01BA5728 4                       MOV     R1, R0,LSR#8
            R1 = (R0 >> 8);
            //ROM:01BA572C 4                       STRB    R1, [R4,#0x3E]
            put_byte((R4 + 0x3E), R1);
            //ROM:01BA5730 4                       STRB    R0, [R4,#0x3F]
            put_byte((R4 + 0x3F), R0);
            //ROM:01BA5734 4                       MOV     R0, R4
            R0 = R4;
            //ROM:01BA5738 4                       MOV     R1, R5
            R1 = R5;
            //ROM:01BA573C 4                       BL      Subcryptography_1 ; R0 óêàçàòåëü íà áëîê äàííûõ
            Subcryptography_1();
            //ROM:01BA573C                                                 ; R1 óêàçàòåëü íà áëîê êîýôôèöèåíòîâ
            //ROM:01BA5740 4                       ADR     R0, unk_1BA541C
            R0 = 0x1BA541C;
            //ROM:01BA5744 4                       MOV     R1, #0xCD
            R1 = 0xCD;
            //ROM:01BA5748 4                       LDMFD   SP!, {R4-R6,LR}
            R4 = get_long(SP + 0x0);
            R5 = get_long(SP + 0x4);
            R6 = get_long(SP + 0x8);
            LR = get_long(SP + 0xC);
            SP = SP + 0x10;
            //ROM:01BA574C 4                       B       SistemPlanirov  ; R0 èìÿ çàäàíè

            //

        }

        private void TCK_Gryptofunc_1()
        {
            //ROM:01BA54A8 4                       STMFD   SP!, {R4-R9,LR}
            put_long(SP - 0x4, LR);
            put_long(SP - 0x8, R9);
            put_long(SP - 0xC, R8);
            put_long(SP - 0x10, R7);
            put_long(SP - 0x14, R6);
            put_long(SP - 0x18, R5);
            put_long(SP - 0x1C, R4);
            SP = SP - 0x1C;
            //ROM:01BA54AC 4                       MOVS    R4, R0          ; R4 áóôåð êîýôôèöèåíòîâ
            R4 = R0;
            Z = (R4) == 0 ? true : false;
            N = ((Int32)R4) < 0 ? true : false;
            //ROM:01BA54B0 4                       MOV     R9, #1
            R9 = 1;
            //ROM:01BA54B4 4                       MOVEQ   R0, R9
            if (Z) R0 = R9;
            //ROM:01BA54B8 4                       MOV     R6, R1          ; R6 âõîäíîé áóôåð
            R6 = R1;
            //ROM:01BA54BC 4                       MOV     R5, R2          ; R5 äëèíà âõîäíîãî áóôåðà
            R5 = R2;
            //ROM:01BA54C0 4                       LDMEQFD SP!, {R4-R9,PC}
            if (Z)
            {
                R4 = get_long(SP + 0x0);
                R5 = get_long(SP + 0x4);
                R6 = get_long(SP + 0x8);
                R7 = get_long(SP + 0xC);
                R8 = get_long(SP + 0x10);
                R9 = get_long(SP + 0x14);
                PC = get_long(SP + 0x18);
                SP = SP + 0x1C;
                return;
            }

            //ROM:01BA54C4 4                       MOV     R1, #0x4D
            R1 = 0x4D;
            //ROM:01BA54C8 4                       ADR     R0, unk_1BA541C
            R0 = 0x1BA541C;
            //ROM:01BA54CC 4                       BL      SistemPlanirov  ; R0 èìÿ çàäàíèÿ
            SistemPlanirov();
            //ROM:01BA54CC                                                 ; R1 êîìàíäà èëè ðàçìåð
            //ROM:01BA54D0 4                       MOV     R8, #0
            R8 = 0;
            //ROM:01BA54D4 4                       ADD     R7, R4, #0x40   ; R7 óêàçàòåëü íà ïåðâûé êîýôôèöèåíò â áóôåðå êîýôôèöèåíòîâ
            R7 = R4 + 0x40;
            //ROM:01BA54D8 4                       B       loc_1BA5524     ; Ïðîâåðÿåò ÷òî ðàçìåð âõîäíîãî áóôåðà áîëüøå íóëÿ
            goto loc_1BA5524;
        //ROM:01BA54DC         ; ---------------------------------------------------------------------------
        //ROM:01BA54DC
        //ROM:01BA54DC         loc_1BA54DC                             ; CODE XREF: TCK_Gryptofunc_1+80j
        loc_1BA54DC:
            //ROM:01BA54DC 4                       CMP     R5, #0x40       ; ñðàâíèâàåò ðàçìåð âõîäíîãî áóôåðà ñ 64
            V = (((Int64)(Int32)R5 - (Int64)(Int32)(0x40)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R5 - (Int64)(Int32)(0x40)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R5 - (Int64)(0x40)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R5 - 0x40) == 0 ? true : false;
            N = ((Int32)(R5 - 0x40)) < 0 ? true : false;
            //ROM:01BA54E0 4                       BCC     loc_1BA559C     ; áóôåð ìåíüøå 64 íî áîëüøå 0
            if (!C) goto loc_1BA559C;
            //ROM:01BA54E4 4                       MOV     R1, R6
            R1 = R6;
            //ROM:01BA54E8 4                       MOV     R0, R4
            R0 = R4;
            //ROM:01BA54EC 4                       MOV     R2, #0x40
            R2 = 0x40;
            //ROM:01BA54F0 4                       BL      aMemCpy
            aMemCpy();
            //ROM:01BA54F4 4                       LDR     R0, [R4,#0x54]
            R0 = get_long((R4 + 0x54));
            //ROM:01BA54F8 4                       ADDS    R0, R0, #0x200
            V = (((Int64)(Int32)R0 + (Int64)(Int32)(0x200)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 + (Int64)(Int32)(0x200)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 + (Int64)(0x200)) > (Int64)(UInt32.MaxValue) ? true : false;
            R0 = R0 + 0x200;
            C = C1;
            Z = (R0) == 0 ? true : false;
            N = ((Int32)R0) < 0 ? true : false;
            //ROM:01BA54FC 4                       STR     R0, [R4,#0x54]
            put_long((R4 + 0x54), R0);
            //ROM:01BA5500 4                       LDREQ   R0, [R4,#0x58]
            if (Z)
            {
                R0 = get_long((R4 + 0x58));
            }
            //ROM:01BA5504 4                       MOV     R1, R7
            R1 = R7;
            //ROM:01BA5508 4                       ADDEQ   R0, R0, #0x40
            if (Z) R0 = R0 + 0x40;
            //ROM:01BA550C 4                       STREQ   R0, [R4,#0x58]
            if (Z)
            {
                put_long((R4 + 0x58), R0);
            }
            //ROM:01BA5510 4                       MOV     R0, R4
            R0 = R4;
            //ROM:01BA5514 4                       BL      Subcryptography_1 ; R0 óêàçàòåëü íà áëîê äàííûõ
            Subcryptography_1();
            //ROM:01BA5514                                                 ; R1 óêàçàòåëü íà áëîê êîýôôèöèåíòîâ
            //ROM:01BA5518 4                       ADD     R6, R6, #0x40
            R6 = R6 + 0x40;
            //ROM:01BA551C 4                       SUB     R5, R5, #0x40
            R5 = R5 - 0x40;
            //ROM:01BA5520 4                       STR     R8, [R4,#0x5C]
            put_long((R4 + 0x5C), R8);
        //ROM:01BA5524
        //ROM:01BA5524         loc_1BA5524                             ; CODE XREF: TCK_Gryptofunc_1+30j
        loc_1BA5524:
            //ROM:01BA5524 4                       CMP     R5, #0          ; Ïðîâåðÿåò ÷òî ðàçìåð âõîäíîãî áóôåðà áîëüøå íóëÿ
            V = (((Int64)(Int32)R5 - (Int64)(Int32)(0)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R5 - (Int64)(Int32)(0)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R5 - (Int64)(0)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R5 - 0) == 0 ? true : false;
            N = ((Int32)(R5 - 0)) < 0 ? true : false;
            //ROM:01BA5528 4                       BNE     loc_1BA54DC     ; ñðàâíèâàåò ðàçìåð âõîäíîãî áóôåðà ñ 64
            if (!Z) goto loc_1BA54DC;
        //ROM:01BA552C
        //ROM:01BA552C         loc_1BA552C                             ; CODE XREF: TCK_Gryptofunc_1+FCj
        loc_1BA552C:
            //ROM:01BA552C 4                       ADR     R0, unk_1BA541C
            R0 = 0x1BA541C;
            //ROM:01BA5530 4                       MOV     R1, #0x77
            R1 = 0x77;
            //ROM:01BA5534 4                       BL      SistemPlanirov  ; R0 èìÿ çàäàíèÿ
            SistemPlanirov();
            //ROM:01BA5534                                                 ; R1 êîìàíäà èëè ðàçìåð
            //ROM:01BA5538 4                       MOV     R0, R8
            R0 = R8;
            //ROM:01BA553C 4                       LDMFD   SP!, {R4-R9,PC}
            R4 = get_long(SP + 0x0);
            R5 = get_long(SP + 0x4);
            R6 = get_long(SP + 0x8);
            R7 = get_long(SP + 0xC);
            R8 = get_long(SP + 0x10);
            R9 = get_long(SP + 0x14);
            PC = get_long(SP + 0x18);
            SP = SP + 0x1C;
            return;
        //ROM:01BA5540         ; ---------------------------------------------------------------------------
        //ROM:01BA5540
        //ROM:01BA5540         loc_1BA5540                             ; CODE XREF: TCK_Gryptofunc_1+F8j
        loc_1BA5540:
            //ROM:01BA5540 4                       LDR     R0, [R4,#0x5C]
            R0 = get_long((R4 + 0x5C));
            //ROM:01BA5544 4                       LDRB    R1, [R6]
            R1 = get_byte(R6);
            //ROM:01BA5548 4                       ADD     R2, R0, #1
            R2 = R0 + 1;
            //ROM:01BA554C 4                       STR     R2, [R4,#0x5C]
            put_long((R4 + 0x5C), R2);
            //ROM:01BA5550 4                       STRB    R1, [R4,R0]
            put_byte((R4 + R0), R1);
            //ROM:01BA5554 4                       LDR     R0, [R4,#0x54]
            R0 = get_long((R4 + 0x54));
            //ROM:01BA5558 4                       SUB     R5, R5, #1
            R5 = R5 - 1;
            //ROM:01BA555C 4                       ADD     R0, R0, #8
            R0 = R0 + 8;
            //ROM:01BA5560 4                       STR     R0, [R4,#0x54]
            put_long((R4 + 0x54), R0);
            //ROM:01BA5564 4                       CMP     R0, #0
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0) == 0 ? true : false;
            N = ((Int32)(R0 - 0)) < 0 ? true : false;
            //ROM:01BA5568 4                       LDREQ   R0, [R4,#0x58]
            if (Z)
            {
                R0 = get_long((R4 + 0x58));
            }
            //ROM:01BA556C 4                       ADDEQ   R0, R0, #1
            if (Z) R0 = R0 + 1;
            //ROM:01BA5570 4                       STREQ   R0, [R4,#0x58]
            if (Z)
            {
                put_long((R4 + 0x58), R0);
            }
            //ROM:01BA5574 4                       CMPEQ   R0, #0
            if (Z)
            {
                V = (((Int64)(Int32)R0 - (Int64)(Int32)(0)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0)) < (Int64)(Int32.MinValue)) ? true : false;
                C1 = ((Int64)R0 - (Int64)(0)) < (Int64)(UInt32.MinValue) ? false : true;
                C = C1;
                Z = (R0 - 0) == 0 ? true : false;
                N = ((Int32)(R0 - 0)) < 0 ? true : false;
            }
            //ROM:01BA5578 4                       STREQ   R9, [R4,#0x60]
            if (Z)
            {
                put_long((R4 + 0x60), R9);
            }
            //ROM:01BA557C 4                       LDR     R0, [R4,#0x5C]
            R0 = get_long((R4 + 0x5C));
            //ROM:01BA5580 4                       CMP     R0, #0x40
            V = (((Int64)(Int32)R0 - (Int64)(Int32)(0x40)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R0 - (Int64)(Int32)(0x40)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R0 - (Int64)(0x40)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R0 - 0x40) == 0 ? true : false;
            N = ((Int32)(R0 - 0x40)) < 0 ? true : false;
            //ROM:01BA5584 4                       BNE     loc_1BA5598
            if (!Z) goto loc_1BA5598;
            //ROM:01BA5588 4                       MOV     R1, R7
            R1 = R7;
            //ROM:01BA558C 4                       MOV     R0, R4
            R0 = R4;
            //ROM:01BA5590 4                       BL      Subcryptography_1 ; R0 óêàçàòåëü íà áëîê äàííûõ
            Subcryptography_1();
            //ROM:01BA5590                                                 ; R1 óêàçàòåëü íà áëîê êîýôôèöèåíòîâ
            //ROM:01BA5594 4                       STR     R8, [R4,#0x5C]
            put_long((R4 + 0x5C), R8);
        //ROM:01BA5598
        //ROM:01BA5598         loc_1BA5598                             ; CODE XREF: TCK_Gryptofunc_1+DCj
        loc_1BA5598:
            //ROM:01BA5598 4                       ADD     R6, R6, #1
            R6 = R6 + 1;
        //ROM:01BA559C
        //ROM:01BA559C         loc_1BA559C                             ; CODE XREF: TCK_Gryptofunc_1+38j
        loc_1BA559C:
            //ROM:01BA559C 4                       CMP     R5, #0          ; áóôåð ìåíüøå 64 íî áîëüøå 0
            V = (((Int64)(Int32)R5 - (Int64)(Int32)(0)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R5 - (Int64)(Int32)(0)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R5 - (Int64)(0)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R5 - 0) == 0 ? true : false;
            N = ((Int32)(R5 - 0)) < 0 ? true : false;
            //ROM:01BA55A0 4                       BNE     loc_1BA5540
            if (!Z) goto loc_1BA5540;
            //ROM:01BA55A4 4                       B       loc_1BA552C
            goto loc_1BA552C;
            //
        }

        private void GotovitKoefficienti()
        {
            //ROM:01BA5430 4                       STMFD   SP!, {R4,R5,LR}
            put_long(SP - 0x4, LR);
            put_long(SP - 0x8, R5);
            put_long(SP - 0xC, R4);
            SP = SP - 0xC;
            //ROM:01BA5434 4                       MOV     R4, R0
            R4 = R0;
            //ROM:01BA5438 4                       ADR     R0, unk_1BA541C
            R0 = 0x1BA541C;
            //ROM:01BA543C 4                       MOV     R1, #0x2E
            R1 = 0x2E;
            //ROM:01BA5440 4                       BL      SistemPlanirov  ; R0 èìÿ çàäàíèÿ
            SistemPlanirov();
            //ROM:01BA5440                                                 ; R1 êîìàíäà èëè ðàçìåð
            //ROM:01BA5444 4                       MOV     R5, #0
            R5 = 0;
            //ROM:01BA5448 4                       STR     R5, [R4,#0x54]
            put_long((R4 + 0x54), R5);
            //ROM:01BA544C 4                       STR     R5, [R4,#0x58]
            put_long((R4 + 0x58), R5);
            //ROM:01BA5450 4                       LDR     R0, =0x67452301
            R0 = 0x67452301;
            //ROM:01BA5454 4                       STR     R5, [R4,#0x5C]
            put_long((R4 + 0x5C), R5);
            //ROM:01BA5458 4                       STR     R0, [R4,#0x40]
            put_long((R4 + 0x40), R0);
            //ROM:01BA545C 4                       LDR     R0, =0xEFCDAB89
            R0 = 0xEFCDAB89;
            //ROM:01BA5460 4                       MOV     R1, #0x3A
            R1 = 0x3A;
            //ROM:01BA5464 4                       STR     R0, [R4,#0x44]
            put_long((R4 + 0x44), R0);
            //ROM:01BA5468 4                       LDR     R0, =0x98BADCFE
            R0 = 0x98BADCFE;
            //ROM:01BA546C 4                       STR     R0, [R4,#0x48]
            put_long((R4 + 0x48), R0);
            //ROM:01BA5470 4                       LDR     R0, =0x10325476
            R0 = 0x10325476;
            //ROM:01BA5474 4                       STR     R0, [R4,#0x4C]
            put_long((R4 + 0x4C), R0);
            //ROM:01BA5478 4                       LDR     R0, =0xC3D2E1F0
            R0 = 0xC3D2E1F0;
            //ROM:01BA547C 4                       STR     R0, [R4,#0x50]
            put_long((R4 + 0x50), R0);
            //ROM:01BA5480 4                       STR     R5, [R4,#0x60]
            put_long((R4 + 0x60), R5);
            //ROM:01BA5484 4                       ADR     R0, unk_1BA541C
            R0 = 0x1BA541C;
            //ROM:01BA5488 4                       BL      SistemPlanirov  ; R0 èìÿ çàäàíèÿ
            SistemPlanirov();
            //ROM:01BA5488                                                 ; R1 êîìàíäà èëè ðàçìåð
            //ROM:01BA548C 4                       MOV     R0, R5
            R0 = R5;
            //ROM:01BA5490 4                       LDMFD   SP!, {R4,R5,PC}
            R4 = get_long(SP + 0x0);
            R5 = get_long(SP + 0x4);
            PC = get_long(SP + 0x8);
            SP = SP + 0xC;

            //
        }

        private void SistemPlanirov()
        {

        }

        private void aMemCpy()
        {
            //ROM:01FAF06C 4                       ORR     R3, R0, R1
            R3 = R0 | R1;
            //ROM:01FAF070 4                       ORR     R3, R3, R2
            R3 = R3 | R2;
            //ROM:01FAF074 4                       TST     R3, #3
            Z = (R3 & 3) == 0 ? true : false;
            N = ((Int32)(R3 & 3)) < 0 ? true : false;
            //ROM:01FAF078 4                       MOVEQ   R3, R2,LSR#2
            if (Z) R3 = (R2 >> 2);
            //ROM:01FAF07C 4                       MOVEQ   R2, R0
            if (Z) R2 = R0;
            //ROM:01FAF080 4                       BEQ     loc_1FAF094
            if (Z) goto loc_1FAF094;
            //ROM:01FAF084 4                       MOV     R3, R0
            R3 = R0;
            //ROM:01FAF088 4                       B       loc_1FAF0B0
            goto loc_1FAF0B0;
        //ROM:01FAF08C         ; ---------------------------------------------------------------------------
        //ROM:01FAF08C
        //ROM:01FAF08C         loc_1FAF08C                             ; CODE XREF: aMemCpy+34j
        loc_1FAF08C:
            //ROM:01FAF08C 4                       LDR     R12, [R1],#4
            R12 = get_long((R1));
            R1 = R1 + 4;
            //ROM:01FAF090 4                       STR     R12, [R2],#4
            put_long((R2), R12);
            R2 = R2 + 4;
        //ROM:01FAF094
        //ROM:01FAF094         loc_1FAF094                             ; CODE XREF: aMemCpy+14j
        loc_1FAF094:
            //ROM:01FAF094 4                       MOV     R12, R3
            R12 = R3;
            //ROM:01FAF098 4                       CMP     R12, #0
            V = (((Int64)(Int32)R12 - (Int64)(Int32)(0)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R12 - (Int64)(Int32)(0)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R12 - (Int64)(0)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R12 - 0) == 0 ? true : false;
            N = ((Int32)(R12 - 0)) < 0 ? true : false;
            //ROM:01FAF09C 4                       SUB     R3, R3, #1
            R3 = R3 - 1;
            //ROM:01FAF0A0 4                       BHI     loc_1FAF08C
            if (C && !Z) goto loc_1FAF08C;
            //ROM:01FAF0A4 4                       RET
            return;
        //ROM:01FAF0A8         ; ---------------------------------------------------------------------------
        //ROM:01FAF0A8
        //ROM:01FAF0A8         loc_1FAF0A8                             ; CODE XREF: aMemCpy+50j
        loc_1FAF0A8:
            //ROM:01FAF0A8 4                       LDRB    R12, [R1],#1
            R12 = get_byte((R1));
            R1 = R1 + 1;
            //ROM:01FAF0AC 4                       STRB    R12, [R3],#1
            put_byte((R3), R12);
            R3 = R3 + 1;
        //ROM:01FAF0B0
        //ROM:01FAF0B0         loc_1FAF0B0                             ; CODE XREF: aMemCpy+1Cj
        loc_1FAF0B0:
            //ROM:01FAF0B0 4                       MOV     R12, R2
            R12 = R2;
            //ROM:01FAF0B4 4                       CMP     R12, #0
            V = (((Int64)(Int32)R12 - (Int64)(Int32)(0)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R12 - (Int64)(Int32)(0)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R12 - (Int64)(0)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R12 - 0) == 0 ? true : false;
            N = ((Int32)(R12 - 0)) < 0 ? true : false;
            //ROM:01FAF0B8 4                       SUB     R2, R2, #1
            R2 = R2 - 1;
            //ROM:01FAF0BC 4                       BHI     loc_1FAF0A8
            if (C && !Z) goto loc_1FAF0A8;
            //ROM:01FAF0C0 4                       RET

            //

        }

        private void Subcryptography_1()
        {
            //ROM:01BA5750 4                       STMFD   SP!, {R4-R6,LR}
            put_long(SP - 0x4, LR);
            put_long(SP - 0x8, R6);
            put_long(SP - 0xC, R5);
            put_long(SP - 0x10, R4);
            SP = SP - 0x10;
            //ROM:01BA5754 4                       SUB     SP, SP, #0x40   ; âûäåëÿåì ìåñòî â ñòåêå ïîä âõîäíûå äàííûå ( 64 áàéòà )
            SP = SP - 0x40;
            //ROM:01BA5758 4                       MOV     R2, #0
            R2 = 0;
        //ROM:01BA575C
        //ROM:01BA575C         loc_1BA575C                             ; CODE XREF: Subcryptography_1+20j
        loc_1BA575C:
            //ROM:01BA575C 4                       LDR     R12, [R0,R2,LSL#2] ; êîïèðóåì 64 áàéòà èç âõîäíûõ äàííûõ â áóôåð ( óêàçàòåëü ñòåêà )
            R12 = get_long((R0 + (R2 << 2)));
            //ROM:01BA5760 4                       STR     R12, [SP,R2,LSL#2]
            put_long((SP + (R2 << 2)), R12);
            //ROM:01BA5764 4                       ADD     R2, R2, #1
            R2 = R2 + 1;
            //ROM:01BA5768 4                       AND     R2, R2, #0xFF
            R2 = R2 & 0xFF;
            //ROM:01BA576C 4                       CMP     R2, #0x10
            V = (((Int64)(Int32)R2 - (Int64)(Int32)(0x10)) > (Int64)(Int32.MaxValue)) || (((Int64)(Int32)R2 - (Int64)(Int32)(0x10)) < (Int64)(Int32.MinValue)) ? true : false;
            C1 = ((Int64)R2 - (Int64)(0x10)) < (Int64)(UInt32.MinValue) ? false : true;
            C = C1;
            Z = (R2 - 0x10) == 0 ? true : false;
            N = ((Int32)(R2 - 0x10)) < 0 ? true : false;
            //ROM:01BA5770 4                       BLT     loc_1BA575C     ; êîïèðóåì 64 áàéòà èç âõîäíûõ äàííûõ â áóôåð ( óêàçàòåëü ñòåêà )
            if (N != V) goto loc_1BA575C;
            //ROM:01BA5774 4                       LDR     R0, [R1,#0xC]   ; êîýôô Ñ3
            R0 = get_long((R1 + 0xC));
            //ROM:01BA5778 4                       LDR     R2, [R1,#8]     ; êîýôô Ñ2
            R2 = get_long((R1 + 8));
            //ROM:01BA577C 4                       LDR     R12, [R1,#4]    ; êîýôô Ñ1
            R12 = get_long((R1 + 4));
            //ROM:01BA5780 4                       EOR     R4, R2, R0      ; c3 xor c2
            R4 = R2 ^ R0;
            //ROM:01BA5784 4                       AND     R4, R4, R12     ; (ñ3 xor ñ2) & c1
            R4 = R4 & R12;
            //ROM:01BA5788 4                       LDR     R3, [R1,#0x10]  ; C4
            R3 = get_long((R1 + 0x10));
            //ROM:01BA578C 4                       LDR     LR, [R1]        ; C0
            LR = get_long(R1);
            //ROM:01BA5790 4                       EOR     R6, R4, R0      ; p1 = c3 xor ( (c3 xor c2) & c1 )
            R6 = R4 ^ R0;
            //ROM:01BA5794 4                       LDR     R4, [SP,#0x50+var_50] ; indata[0]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA5798 4                       MOV     R12, R12,ROR#2  ; c1 ror 2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA579C 4                       ADD     R4, R6, R4
            R4 = R6 + R4;
            //ROM:01BA57A0 4                       ADD     R4, R4, LR,ROR#27
            R4 = R4 + ((LR >> 27) | (LR << 5));
            //ROM:01BA57A4 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA57A8 4                       EOR     R4, R12, R2
            R4 = R12 ^ R2;
            //ROM:01BA57AC 4                       AND     R4, R4, LR
            R4 = R4 & LR;
            //ROM:01BA57B0 4                       ADD     R3, R3, #0x99
            R3 = R3 + 0x99;
            //ROM:01BA57B4 4                       ADD     R3, R3, #0x7900
            R3 = R3 + 0x7900;
            //ROM:01BA57B8 4                       LDR     R5, [SP,#0x50+var_4C]
            R5 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA57BC 4                       ADD     R3, R3, #0x820000
            R3 = R3 + 0x820000;
            //ROM:01BA57C0 4                       EOR     R4, R4, R2
            R4 = R4 ^ R2;
            //ROM:01BA57C4 4                       ADD     R5, R4, R5
            R5 = R4 + R5;
            //ROM:01BA57C8 4                       ADD     R3, R3, #0x5A000000
            R3 = R3 + 0x5A000000;
            //ROM:01BA57CC 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA57D0 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA57D4 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA57D8 4                       EOR     R4, LR, R12
            R4 = LR ^ R12;
            //ROM:01BA57DC 4                       AND     R4, R4, R3
            R4 = R4 & R3;
            //ROM:01BA57E0 4                       EOR     R5, R4, R12
            R5 = R4 ^ R12;
            //ROM:01BA57E4 4                       ADD     R0, R0, #0x99
            R0 = R0 + 0x99;
            //ROM:01BA57E8 4                       ADD     R0, R0, #0x7900
            R0 = R0 + 0x7900;
            //ROM:01BA57EC 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA57F0 4                       ADD     R0, R0, #0x820000
            R0 = R0 + 0x820000;
            //ROM:01BA57F4 4                       ADD     R0, R0, #0x5A000000
            R0 = R0 + 0x5A000000;
            //ROM:01BA57F8 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA57FC 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA5800 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA5804 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA5808 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA580C 4                       AND     R4, R4, R0
            R4 = R4 & R0;
            //ROM:01BA5810 4                       EOR     R5, R4, LR
            R5 = R4 ^ LR;
            //ROM:01BA5814 4                       ADD     R2, R2, #0x99
            R2 = R2 + 0x99;
            //ROM:01BA5818 4                       ADD     R2, R2, #0x7900
            R2 = R2 + 0x7900;
            //ROM:01BA581C 4                       LDR     R4, [SP,#0x50+var_44]
            R4 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA5820 4                       ADD     R2, R2, #0x820000
            R2 = R2 + 0x820000;
            //ROM:01BA5824 4                       ADD     R2, R2, #0x5A000000
            R2 = R2 + 0x5A000000;
            //ROM:01BA5828 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA582C 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA5830 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA5834 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA5838 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA583C 4                       AND     R4, R4, R2
            R4 = R4 & R2;
            //ROM:01BA5840 4                       EOR     R5, R4, R3
            R5 = R4 ^ R3;
            //ROM:01BA5844 4                       ADD     R12, R12, #0x99
            R12 = R12 + 0x99;
            //ROM:01BA5848 4                       ADD     R12, R12, #0x7900
            R12 = R12 + 0x7900;
            //ROM:01BA584C 4                       LDR     R4, [SP,#0x50+var_40]
            R4 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA5850 4                       ADD     R12, R12, #0x820000
            R12 = R12 + 0x820000;
            //ROM:01BA5854 4                       ADD     R12, R12, #0x5A000000
            R12 = R12 + 0x5A000000;
            //ROM:01BA5858 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA585C 4                       ADD     R4, R5, R12,ROR#27
            R4 = R5 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA5860 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA5864 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA5868 4                       EOR     R4, R2, R0
            R4 = R2 ^ R0;
            //ROM:01BA586C 4                       AND     R4, R4, R12
            R4 = R4 & R12;
            //ROM:01BA5870 4                       ADD     LR, LR, #0x99
            LR = LR + 0x99;
            //ROM:01BA5874 4                       ADD     LR, LR, #0x7900
            LR = LR + 0x7900;
            //ROM:01BA5878 4                       LDR     R5, [SP,#0x50+var_3C]
            R5 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA587C 4                       ADD     LR, LR, #0x820000
            LR = LR + 0x820000;
            //ROM:01BA5880 4                       EOR     R4, R4, R0
            R4 = R4 ^ R0;
            //ROM:01BA5884 4                       ADD     R5, R4, R5
            R5 = R4 + R5;
            //ROM:01BA5888 4                       ADD     LR, LR, #0x5A000000
            LR = LR + 0x5A000000;
            //ROM:01BA588C 4                       ADD     R4, R5, LR,ROR#27
            R4 = R5 + ((LR >> 27) | (LR << 5));
            //ROM:01BA5890 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA5894 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA5898 4                       EOR     R4, R12, R2
            R4 = R12 ^ R2;
            //ROM:01BA589C 4                       AND     R4, R4, LR
            R4 = R4 & LR;
            //ROM:01BA58A0 4                       ADD     R3, R3, #0x99
            R3 = R3 + 0x99;
            //ROM:01BA58A4 4                       ADD     R3, R3, #0x7900
            R3 = R3 + 0x7900;
            //ROM:01BA58A8 4                       LDR     R5, [SP,#0x50+var_38]
            R5 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA58AC 4                       ADD     R3, R3, #0x820000
            R3 = R3 + 0x820000;
            //ROM:01BA58B0 4                       EOR     R4, R4, R2
            R4 = R4 ^ R2;
            //ROM:01BA58B4 4                       ADD     R5, R4, R5
            R5 = R4 + R5;
            //ROM:01BA58B8 4                       ADD     R3, R3, #0x5A000000
            R3 = R3 + 0x5A000000;
            //ROM:01BA58BC 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA58C0 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA58C4 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA58C8 4                       EOR     R4, LR, R12
            R4 = LR ^ R12;
            //ROM:01BA58CC 4                       AND     R4, R4, R3
            R4 = R4 & R3;
            //ROM:01BA58D0 4                       ADD     R0, R0, #0x99
            R0 = R0 + 0x99;
            //ROM:01BA58D4 4                       LDR     R5, [SP,#0x50+var_34]
            R5 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA58D8 4                       ADD     R0, R0, #0x7900
            R0 = R0 + 0x7900;
            //ROM:01BA58DC 4                       EOR     R4, R4, R12
            R4 = R4 ^ R12;
            //ROM:01BA58E0 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01BA58E4 4                       ADD     R0, R0, #0x820000
            R0 = R0 + 0x820000;
            //ROM:01BA58E8 4                       ADD     R0, R0, #0x5A000000
            R0 = R0 + 0x5A000000;
            //ROM:01BA58EC 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA58F0 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA58F4 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA58F8 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA58FC 4                       AND     R4, R4, R0
            R4 = R4 & R0;
            //ROM:01BA5900 4                       ADD     R2, R2, #0x99
            R2 = R2 + 0x99;
            //ROM:01BA5904 4                       ADD     R2, R2, #0x7900
            R2 = R2 + 0x7900;
            //ROM:01BA5908 4                       LDR     R5, [SP,#0x50+var_30]
            R5 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA590C 4                       ADD     R2, R2, #0x820000
            R2 = R2 + 0x820000;
            //ROM:01BA5910 4                       EOR     R4, R4, LR
            R4 = R4 ^ LR;
            //ROM:01BA5914 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01BA5918 4                       ADD     R2, R2, #0x5A000000
            R2 = R2 + 0x5A000000;
            //ROM:01BA591C 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA5920 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA5924 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA5928 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA592C 4                       AND     R4, R4, R2
            R4 = R4 & R2;
            //ROM:01BA5930 4                       EOR     R5, R4, R3
            R5 = R4 ^ R3;
            //ROM:01BA5934 4                       ADD     R12, R12, #0x99
            R12 = R12 + 0x99;
            //ROM:01BA5938 4                       ADD     R12, R12, #0x7900
            R12 = R12 + 0x7900;
            //ROM:01BA593C 4                       LDR     R4, [SP,#0x50+var_2C]
            R4 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA5940 4                       ADD     R12, R12, #0x820000
            R12 = R12 + 0x820000;
            //ROM:01BA5944 4                       ADD     R12, R12, #0x5A000000
            R12 = R12 + 0x5A000000;
            //ROM:01BA5948 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA594C 4                       ADD     R4, R4, R12,ROR#27
            R4 = R4 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA5950 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA5954 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA5958 4                       EOR     R4, R2, R0
            R4 = R2 ^ R0;
            //ROM:01BA595C 4                       AND     R4, R4, R12
            R4 = R4 & R12;
            //ROM:01BA5960 4                       EOR     R5, R4, R0
            R5 = R4 ^ R0;
            //ROM:01BA5964 4                       ADD     LR, LR, #0x99
            LR = LR + 0x99;
            //ROM:01BA5968 4                       ADD     LR, LR, #0x7900
            LR = LR + 0x7900;
            //ROM:01BA596C 4                       LDR     R4, [SP,#0x50+var_28]
            R4 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA5970 4                       ADD     LR, LR, #0x820000
            LR = LR + 0x820000;
            //ROM:01BA5974 4                       ADD     LR, LR, #0x5A000000
            LR = LR + 0x5A000000;
            //ROM:01BA5978 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA597C 4                       ADD     R4, R5, LR,ROR#27
            R4 = R5 + ((LR >> 27) | (LR << 5));
            //ROM:01BA5980 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA5984 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA5988 4                       EOR     R4, R12, R2
            R4 = R12 ^ R2;
            //ROM:01BA598C 4                       AND     R4, R4, LR
            R4 = R4 & LR;
            //ROM:01BA5990 4                       EOR     R5, R4, R2
            R5 = R4 ^ R2;
            //ROM:01BA5994 4                       ADD     R3, R3, #0x99
            R3 = R3 + 0x99;
            //ROM:01BA5998 4                       ADD     R3, R3, #0x7900
            R3 = R3 + 0x7900;
            //ROM:01BA599C 4                       LDR     R4, [SP,#0x50+var_24]
            R4 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA59A0 4                       ADD     R3, R3, #0x820000
            R3 = R3 + 0x820000;
            //ROM:01BA59A4 4                       ADD     R3, R3, #0x5A000000
            R3 = R3 + 0x5A000000;
            //ROM:01BA59A8 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA59AC 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA59B0 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA59B4 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA59B8 4                       EOR     R4, LR, R12
            R4 = LR ^ R12;
            //ROM:01BA59BC 4                       AND     R4, R4, R3
            R4 = R4 & R3;
            //ROM:01BA59C0 4                       ADD     R0, R0, #0x99
            R0 = R0 + 0x99;
            //ROM:01BA59C4 4                       LDR     R5, [SP,#0x50+var_20]
            R5 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA59C8 4                       ADD     R0, R0, #0x7900
            R0 = R0 + 0x7900;
            //ROM:01BA59CC 4                       EOR     R4, R4, R12
            R4 = R4 ^ R12;
            //ROM:01BA59D0 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01BA59D4 4                       ADD     R0, R0, #0x820000
            R0 = R0 + 0x820000;
            //ROM:01BA59D8 4                       ADD     R0, R0, #0x5A000000
            R0 = R0 + 0x5A000000;
            //ROM:01BA59DC 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA59E0 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA59E4 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA59E8 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA59EC 4                       AND     R4, R4, R0
            R4 = R4 & R0;
            //ROM:01BA59F0 4                       ADD     R2, R2, #0x99
            R2 = R2 + 0x99;
            //ROM:01BA59F4 4                       LDR     R5, [SP,#0x50+var_1C]
            R5 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA59F8 4                       ADD     R2, R2, #0x7900
            R2 = R2 + 0x7900;
            //ROM:01BA59FC 4                       EOR     R4, R4, LR
            R4 = R4 ^ LR;
            //ROM:01BA5A00 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01BA5A04 4                       ADD     R2, R2, #0x820000
            R2 = R2 + 0x820000;
            //ROM:01BA5A08 4                       ADD     R2, R2, #0x5A000000
            R2 = R2 + 0x5A000000;
            //ROM:01BA5A0C 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA5A10 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA5A14 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA5A18 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA5A1C 4                       AND     R4, R4, R2
            R4 = R4 & R2;
            //ROM:01BA5A20 4                       ADD     R12, R12, #0x99
            R12 = R12 + 0x99;
            //ROM:01BA5A24 4                       LDR     R5, [SP,#0x50+var_18]
            R5 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA5A28 4                       ADD     R12, R12, #0x7900
            R12 = R12 + 0x7900;
            //ROM:01BA5A2C 4                       EOR     R4, R4, R3
            R4 = R4 ^ R3;
            //ROM:01BA5A30 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01BA5A34 4                       ADD     R12, R12, #0x820000
            R12 = R12 + 0x820000;
            //ROM:01BA5A38 4                       ADD     R12, R12, #0x5A000000
            R12 = R12 + 0x5A000000;
            //ROM:01BA5A3C 4                       ADD     R4, R4, R12,ROR#27
            R4 = R4 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA5A40 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA5A44 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA5A48 4                       EOR     R4, R2, R0
            R4 = R2 ^ R0;
            //ROM:01BA5A4C 4                       AND     R4, R4, R12
            R4 = R4 & R12;
            //ROM:01BA5A50 4                       ADD     LR, LR, #0x99
            LR = LR + 0x99;
            //ROM:01BA5A54 4                       LDR     R5, [SP,#0x50+var_14]
            R5 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA5A58 4                       ADD     LR, LR, #0x7900
            LR = LR + 0x7900;
            //ROM:01BA5A5C 4                       EOR     R4, R4, R0
            R4 = R4 ^ R0;
            //ROM:01BA5A60 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01BA5A64 4                       ADD     LR, LR, #0x820000
            LR = LR + 0x820000;
            //ROM:01BA5A68 4                       ADD     LR, LR, #0x5A000000
            LR = LR + 0x5A000000;
            //ROM:01BA5A6C 4                       ADD     R4, R4, LR,ROR#27
            R4 = R4 + ((LR >> 27) | (LR << 5));
            //ROM:01BA5A70 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA5A74 4                       ADD     R3, R3, #0x99
            R3 = R3 + 0x99;
            //ROM:01BA5A78 4                       ADD     R3, R3, #0x7900
            R3 = R3 + 0x7900;
            //ROM:01BA5A7C 4                       LDR     R4, [SP,#0x50+var_1C]
            R4 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA5A80 4                       LDR     R5, [SP,#0x50+var_30]
            R5 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA5A84 4                       ADD     R3, R3, #0x820000
            R3 = R3 + 0x820000;
            //ROM:01BA5A88 4                       ADD     R3, R3, #0x5A000000
            R3 = R3 + 0x5A000000;
            //ROM:01BA5A8C 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA5A90 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5A94 4                       LDR     R5, [SP,#0x50+var_48]
            R5 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA5A98 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA5A9C 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA5AA0 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5AA4 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5AA8 4                       EOR     R4, R12, R2
            R4 = R12 ^ R2;
            //ROM:01BA5AAC 4                       AND     R4, R4, LR
            R4 = R4 & LR;
            //ROM:01BA5AB0 4                       EOR     R4, R4, R2
            R4 = R4 ^ R2;
            //ROM:01BA5AB4 4                       STR     R5, [SP,#0x50+var_50]
            put_long((SP + 0x50 - 0x50), R5);
            //ROM:01BA5AB8 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA5ABC 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA5AC0 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA5AC4 4                       LDR     R4, [SP,#0x50+var_2C]
            R4 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA5AC8 4                       LDR     R5, [SP,#0x50+var_18]
            R5 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA5ACC 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA5AD0 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5AD4 4                       LDR     R4, [SP,#0x50+var_44]
            R4 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA5AD8 4                       ADD     R0, R0, #0x99
            R0 = R0 + 0x99;
            //ROM:01BA5ADC 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5AE0 4                       LDR     R4, [SP,#0x50+var_4C]
            R4 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA5AE4 4                       ADD     R0, R0, #0x7900
            R0 = R0 + 0x7900;
            //ROM:01BA5AE8 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5AEC 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5AF0 4                       EOR     R4, LR, R12
            R4 = LR ^ R12;
            //ROM:01BA5AF4 4                       AND     R4, R4, R3
            R4 = R4 & R3;
            //ROM:01BA5AF8 4                       EOR     R4, R4, R12
            R4 = R4 ^ R12;
            //ROM:01BA5AFC 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA5B00 4                       ADD     R0, R0, #0x820000
            R0 = R0 + 0x820000;
            //ROM:01BA5B04 4                       ADD     R0, R0, #0x5A000000
            R0 = R0 + 0x5A000000;
            //ROM:01BA5B08 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA5B0C 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA5B10 4                       STR     R5, [SP,#0x50+var_4C]
            put_long((SP + 0x50 - 0x4C), R5);
            //ROM:01BA5B14 4                       LDR     R5, [SP,#0x50+var_14]
            R5 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA5B18 4                       LDR     R4, [SP,#0x50+var_28]
            R4 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA5B1C 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA5B20 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5B24 4                       LDR     R5, [SP,#0x50+var_40]
            R5 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA5B28 4                       ADD     R2, R2, #0x99
            R2 = R2 + 0x99;
            //ROM:01BA5B2C 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA5B30 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA5B34 4                       ADD     R2, R2, #0x7900
            R2 = R2 + 0x7900;
            //ROM:01BA5B38 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5B3C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5B40 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA5B44 4                       AND     R4, R4, R0
            R4 = R4 & R0;
            //ROM:01BA5B48 4                       EOR     R4, R4, LR
            R4 = R4 ^ LR;
            //ROM:01BA5B4C 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA5B50 4                       ADD     R2, R2, #0x820000
            R2 = R2 + 0x820000;
            //ROM:01BA5B54 4                       ADD     R2, R2, #0x5A000000
            R2 = R2 + 0x5A000000;
            //ROM:01BA5B58 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA5B5C 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA5B60 4                       STR     R5, [SP,#0x50+var_48]
            put_long((SP + 0x50 - 0x48), R5);
            //ROM:01BA5B64 4                       LDR     R5, [SP,#0x50+var_50]
            R5 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA5B68 4                       LDR     R4, [SP,#0x50+var_24]
            R4 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA5B6C 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA5B70 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA5B74 4                       LDR     R4, [SP,#0x50+var_3C]
            R4 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA5B78 4                       ADD     R12, R12, #0x99
            R12 = R12 + 0x99;
            //ROM:01BA5B7C 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5B80 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA5B84 4                       ADD     R12, R12, #0x7900
            R12 = R12 + 0x7900;
            //ROM:01BA5B88 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5B8C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5B90 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA5B94 4                       AND     R4, R4, R2
            R4 = R4 & R2;
            //ROM:01BA5B98 4                       EOR     R4, R4, R3
            R4 = R4 ^ R3;
            //ROM:01BA5B9C 4                       STR     R5, [SP,#0x50+var_44]
            put_long((SP + 0x50 - 0x44), R5);
            //ROM:01BA5BA0 4                       ADD     R12, R12, #0x820000
            R12 = R12 + 0x820000;
            //ROM:01BA5BA4 4                       ADD     R12, R12, #0x5A000000
            R12 = R12 + 0x5A000000;
            //ROM:01BA5BA8 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA5BAC 4                       ADD     R4, R5, R12,ROR#27
            R4 = R5 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA5BB0 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA5BB4 4                       LDR     R4, [SP,#0x50+var_4C]
            R4 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA5BB8 4                       LDR     R5, [SP,#0x50+var_20]
            R5 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA5BBC 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA5BC0 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5BC4 4                       LDR     R5, [SP,#0x50+var_38]
            R5 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA5BC8 4                       ADD     LR, LR, #0x99
            LR = LR + 0x99;
            //ROM:01BA5BCC 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5BD0 4                       LDR     R5, [SP,#0x50+var_40]
            R5 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA5BD4 4                       ADD     LR, LR, #0x7900
            LR = LR + 0x7900;
            //ROM:01BA5BD8 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5BDC 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5BE0 4                       EOR     R4, R12, R2
            R4 = R12 ^ R2;
            //ROM:01BA5BE4 4                       EOR     R4, R4, R0
            R4 = R4 ^ R0;
            //ROM:01BA5BE8 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA5BEC 4                       ADD     LR, LR, #0x820000
            LR = LR + 0x820000;
            //ROM:01BA5BF0 4                       ADD     LR, LR, #0x5A000000
            LR = LR + 0x5A000000;
            //ROM:01BA5BF4 4                       ADD     R4, R4, LR,ROR#27
            R4 = R4 + ((LR >> 27) | (LR << 5));
            //ROM:01BA5BF8 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA5BFC 4                       STR     R5, [SP,#0x50+var_40]
            put_long((SP + 0x50 - 0x40), R5);
            //ROM:01BA5C00 4                       LDR     R5, [SP,#0x50+var_48]
            R5 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA5C04 4                       LDR     R4, [SP,#0x50+var_1C]
            R4 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA5C08 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA5C0C 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5C10 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA5C14 4                       SUB     R3, R3, #0x5F
            R3 = R3 - 0x5F;
            //ROM:01BA5C18 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5C1C 4                       LDR     R4, [SP,#0x50+var_3C]
            R4 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA5C20 4                       ADD     R3, R3, #0x1EC00
            R3 = R3 + 0x1EC00;
            //ROM:01BA5C24 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5C28 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5C2C 4                       EOR     R4, LR, R12
            R4 = LR ^ R12;
            //ROM:01BA5C30 4                       EOR     R4, R4, R2
            R4 = R4 ^ R2;
            //ROM:01BA5C34 4                       STR     R5, [SP,#0x50+var_3C]
            put_long((SP + 0x50 - 0x3C), R5);
            //ROM:01BA5C38 4                       SUB     R3, R3, #0x1280000
            R3 = R3 - 0x1280000;
            //ROM:01BA5C3C 4                       ADD     R3, R3, #0x70000000
            R3 = R3 + 0x70000000;
            //ROM:01BA5C40 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA5C44 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA5C48 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA5C4C 4                       LDR     R4, [SP,#0x50+var_18]
            R4 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA5C50 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA5C54 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA5C58 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5C5C 4                       LDR     R4, [SP,#0x50+var_30]
            R4 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA5C60 4                       SUB     R0, R0, #0x5F
            R0 = R0 - 0x5F;
            //ROM:01BA5C64 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5C68 4                       LDR     R5, [SP,#0x50+var_38]
            R5 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA5C6C 4                       ADD     R0, R0, #0x1EC00
            R0 = R0 + 0x1EC00;
            //ROM:01BA5C70 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5C74 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5C78 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA5C7C 4                       EOR     R4, R4, R12
            R4 = R4 ^ R12;
            //ROM:01BA5C80 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA5C84 4                       SUB     R0, R0, #0x1280000
            R0 = R0 - 0x1280000;
            //ROM:01BA5C88 4                       ADD     R0, R0, #0x70000000
            R0 = R0 + 0x70000000;
            //ROM:01BA5C8C 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA5C90 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA5C94 4                       STR     R5, [SP,#0x50+var_38]
            put_long((SP + 0x50 - 0x38), R5);
            //ROM:01BA5C98 4                       LDR     R5, [SP,#0x50+var_40]
            R5 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA5C9C 4                       LDR     R4, [SP,#0x50+var_14]
            R4 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA5CA0 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA5CA4 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5CA8 4                       LDR     R4, [SP,#0x50+var_2C]
            R4 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA5CAC 4                       SUB     R2, R2, #0x5F
            R2 = R2 - 0x5F;
            //ROM:01BA5CB0 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5CB4 4                       LDR     R5, [SP,#0x50+var_34]
            R5 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA5CB8 4                       ADD     R2, R2, #0x1EC00
            R2 = R2 + 0x1EC00;
            //ROM:01BA5CBC 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5CC0 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5CC4 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA5CC8 4                       EOR     R4, R4, LR
            R4 = R4 ^ LR;
            //ROM:01BA5CCC 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA5CD0 4                       SUB     R2, R2, #0x1280000
            R2 = R2 - 0x1280000;
            //ROM:01BA5CD4 4                       ADD     R2, R2, #0x70000000
            R2 = R2 + 0x70000000;
            //ROM:01BA5CD8 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA5CDC 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA5CE0 4                       STR     R5, [SP,#0x50+var_34]
            put_long((SP + 0x50 - 0x34), R5);
            //ROM:01BA5CE4 4                       LDR     R5, [SP,#0x50+var_3C]
            R5 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA5CE8 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA5CEC 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA5CF0 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5CF4 4                       LDR     R4, [SP,#0x50+var_28]
            R4 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA5CF8 4                       SUB     R12, R12, #0x5F
            R12 = R12 - 0x5F;
            //ROM:01BA5CFC 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5D00 4                       LDR     R5, [SP,#0x50+var_30]
            R5 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA5D04 4                       ADD     R12, R12, #0x1EC00
            R12 = R12 + 0x1EC00;
            //ROM:01BA5D08 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5D0C 4                       EOR     R5, R2, R0
            R5 = R2 ^ R0;
            //ROM:01BA5D10 4                       MOV     R4, R4,ROR#31
            R4 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5D14 4                       EOR     R5, R5, R3
            R5 = R5 ^ R3;
            //ROM:01BA5D18 4                       SUB     R12, R12, #0x1280000
            R12 = R12 - 0x1280000;
            //ROM:01BA5D1C 4                       ADD     R12, R12, #0x70000000
            R12 = R12 + 0x70000000;
            //ROM:01BA5D20 4                       ADD     R5, R4, R5
            R5 = R4 + R5;
            //ROM:01BA5D24 4                       STR     R4, [SP,#0x50+var_30]
            put_long((SP + 0x50 - 0x30), R4);
            //ROM:01BA5D28 4                       ADD     R4, R5, R12,ROR#27
            R4 = R5 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA5D2C 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA5D30 4                       LDR     R4, [SP,#0x50+var_4C]
            R4 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA5D34 4                       LDR     R5, [SP,#0x50+var_38]
            R5 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA5D38 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA5D3C 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5D40 4                       LDR     R5, [SP,#0x50+var_24]
            R5 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA5D44 4                       SUB     LR, LR, #0x5F
            LR = LR - 0x5F;
            //ROM:01BA5D48 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5D4C 4                       LDR     R5, [SP,#0x50+var_2C]
            R5 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA5D50 4                       ADD     LR, LR, #0x1EC00
            LR = LR + 0x1EC00;
            //ROM:01BA5D54 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5D58 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5D5C 4                       EOR     R4, R12, R2
            R4 = R12 ^ R2;
            //ROM:01BA5D60 4                       EOR     R4, R4, R0
            R4 = R4 ^ R0;
            //ROM:01BA5D64 4                       STR     R5, [SP,#0x50+var_2C]
            put_long((SP + 0x50 - 0x2C), R5);
            //ROM:01BA5D68 4                       SUB     LR, LR, #0x1280000
            LR = LR - 0x1280000;
            //ROM:01BA5D6C 4                       ADD     LR, LR, #0x70000000
            LR = LR + 0x70000000;
            //ROM:01BA5D70 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA5D74 4                       ADD     R4, R5, LR,ROR#27
            R4 = R5 + ((LR >> 27) | (LR << 5));
            //ROM:01BA5D78 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA5D7C 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA5D80 4                       LDR     R5, [SP,#0x50+var_34]
            R5 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA5D84 4                       SUB     R3, R3, #0x5F
            R3 = R3 - 0x5F;
            //ROM:01BA5D88 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5D8C 4                       LDR     R4, [SP,#0x50+var_20]
            R4 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA5D90 4                       ADD     R3, R3, #0x1EC00
            R3 = R3 + 0x1EC00;
            //ROM:01BA5D94 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5D98 4                       LDR     R5, [SP,#0x50+var_28]
            R5 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA5D9C 4                       SUB     R3, R3, #0x1280000
            R3 = R3 - 0x1280000;
            //ROM:01BA5DA0 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5DA4 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5DA8 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA5DAC 4                       EOR     R4, LR, R12
            R4 = LR ^ R12;
            //ROM:01BA5DB0 4                       ADD     R3, R3, #0x70000000
            R3 = R3 + 0x70000000;
            //ROM:01BA5DB4 4                       EOR     R4, R4, R2
            R4 = R4 ^ R2;
            //ROM:01BA5DB8 4                       STR     R5, [SP,#0x50+var_28]
            put_long((SP + 0x50 - 0x28), R5);
            //ROM:01BA5DBC 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA5DC0 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA5DC4 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA5DC8 4                       LDR     R4, [SP,#0x50+var_30]
            R4 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA5DCC 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA5DD0 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA5DD4 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA5DD8 4                       LDR     R4, [SP,#0x50+var_1C]
            R4 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA5DDC 4                       SUB     R0, R0, #0x5F
            R0 = R0 - 0x5F;
            //ROM:01BA5DE0 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5DE4 4                       LDR     R5, [SP,#0x50+var_24]
            R5 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA5DE8 4                       ADD     R0, R0, #0x1EC00
            R0 = R0 + 0x1EC00;
            //ROM:01BA5DEC 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5DF0 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5DF4 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA5DF8 4                       EOR     R4, R4, R12
            R4 = R4 ^ R12;
            //ROM:01BA5DFC 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA5E00 4                       SUB     R0, R0, #0x1280000
            R0 = R0 - 0x1280000;
            //ROM:01BA5E04 4                       ADD     R0, R0, #0x70000000
            R0 = R0 + 0x70000000;
            //ROM:01BA5E08 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA5E0C 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA5E10 4                       STR     R5, [SP,#0x50+var_24]
            put_long((SP + 0x50 - 0x24), R5);
            //ROM:01BA5E14 4                       LDR     R5, [SP,#0x50+var_40]
            R5 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA5E18 4                       LDR     R4, [SP,#0x50+var_2C]
            R4 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA5E1C 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA5E20 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5E24 4                       LDR     R5, [SP,#0x50+var_18]
            R5 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA5E28 4                       SUB     R2, R2, #0x5F
            R2 = R2 - 0x5F;
            //ROM:01BA5E2C 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA5E30 4                       LDR     R4, [SP,#0x50+var_20]
            R4 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA5E34 4                       ADD     R2, R2, #0x1EC00
            R2 = R2 + 0x1EC00;
            //ROM:01BA5E38 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5E3C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5E40 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA5E44 4                       EOR     R4, R4, LR
            R4 = R4 ^ LR;
            //ROM:01BA5E48 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA5E4C 4                       SUB     R2, R2, #0x1280000
            R2 = R2 - 0x1280000;
            //ROM:01BA5E50 4                       ADD     R2, R2, #0x70000000
            R2 = R2 + 0x70000000;
            //ROM:01BA5E54 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA5E58 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA5E5C 4                       STR     R5, [SP,#0x50+var_20]
            put_long((SP + 0x50 - 0x20), R5);
            //ROM:01BA5E60 4                       LDR     R5, [SP,#0x50+var_28]
            R5 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA5E64 4                       LDR     R4, [SP,#0x50+var_3C]
            R4 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA5E68 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA5E6C 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5E70 4                       LDR     R4, [SP,#0x50+var_14]
            R4 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA5E74 4                       SUB     R12, R12, #0x5F
            R12 = R12 - 0x5F;
            //ROM:01BA5E78 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5E7C 4                       LDR     R5, [SP,#0x50+var_1C]
            R5 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA5E80 4                       ADD     R12, R12, #0x1EC00
            R12 = R12 + 0x1EC00;
            //ROM:01BA5E84 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5E88 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5E8C 4                       EOR     R4, R2, R0
            R4 = R2 ^ R0;
            //ROM:01BA5E90 4                       EOR     R4, R4, R3
            R4 = R4 ^ R3;
            //ROM:01BA5E94 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA5E98 4                       SUB     R12, R12, #0x1280000
            R12 = R12 - 0x1280000;
            //ROM:01BA5E9C 4                       ADD     R12, R12, #0x70000000
            R12 = R12 + 0x70000000;
            //ROM:01BA5EA0 4                       ADD     R4, R4, R12,ROR#27
            R4 = R4 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA5EA4 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA5EA8 4                       STR     R5, [SP,#0x50+var_1C]
            put_long((SP + 0x50 - 0x1C), R5);
            //ROM:01BA5EAC 4                       LDR     R5, [SP,#0x50+var_38]
            R5 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA5EB0 4                       LDR     R4, [SP,#0x50+var_24]
            R4 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA5EB4 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA5EB8 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA5EBC 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA5EC0 4                       SUB     LR, LR, #0x5F
            LR = LR - 0x5F;
            //ROM:01BA5EC4 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5EC8 4                       LDR     R5, [SP,#0x50+var_18]
            R5 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA5ECC 4                       ADD     LR, LR, #0x1EC00
            LR = LR + 0x1EC00;
            //ROM:01BA5ED0 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5ED4 4                       EOR     R5, R12, R2
            R5 = R12 ^ R2;
            //ROM:01BA5ED8 4                       MOV     R4, R4,ROR#31
            R4 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5EDC 4                       EOR     R5, R5, R0
            R5 = R5 ^ R0;
            //ROM:01BA5EE0 4                       SUB     LR, LR, #0x1280000
            LR = LR - 0x1280000;
            //ROM:01BA5EE4 4                       ADD     LR, LR, #0x70000000
            LR = LR + 0x70000000;
            //ROM:01BA5EE8 4                       ADD     R5, R4, R5
            R5 = R4 + R5;
            //ROM:01BA5EEC 4                       STR     R4, [SP,#0x50+var_18]
            put_long((SP + 0x50 - 0x18), R4);
            //ROM:01BA5EF0 4                       ADD     R4, R5, LR,ROR#27
            R4 = R5 + ((LR >> 27) | (LR << 5));
            //ROM:01BA5EF4 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA5EF8 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA5EFC 4                       LDR     R5, [SP,#0x50+var_20]
            R5 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA5F00 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA5F04 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5F08 4                       LDR     R4, [SP,#0x50+var_4C]
            R4 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA5F0C 4                       SUB     R3, R3, #0x5F
            R3 = R3 - 0x5F;
            //ROM:01BA5F10 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5F14 4                       LDR     R4, [SP,#0x50+var_14]
            R4 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA5F18 4                       ADD     R3, R3, #0x1EC00
            R3 = R3 + 0x1EC00;
            //ROM:01BA5F1C 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5F20 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5F24 4                       EOR     R4, LR, R12
            R4 = LR ^ R12;
            //ROM:01BA5F28 4                       EOR     R4, R4, R2
            R4 = R4 ^ R2;
            //ROM:01BA5F2C 4                       STR     R5, [SP,#0x50+var_14]
            put_long((SP + 0x50 - 0x14), R5);
            //ROM:01BA5F30 4                       SUB     R3, R3, #0x1280000
            R3 = R3 - 0x1280000;
            //ROM:01BA5F34 4                       ADD     R3, R3, #0x70000000
            R3 = R3 + 0x70000000;
            //ROM:01BA5F38 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA5F3C 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA5F40 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA5F44 4                       LDR     R4, [SP,#0x50+var_1C]
            R4 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA5F48 4                       LDR     R5, [SP,#0x50+var_30]
            R5 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA5F4C 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA5F50 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA5F54 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA5F58 4                       SUB     R0, R0, #0x5F
            R0 = R0 - 0x5F;
            //ROM:01BA5F5C 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA5F60 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA5F64 4                       ADD     R0, R0, #0x1EC00
            R0 = R0 + 0x1EC00;
            //ROM:01BA5F68 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5F6C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5F70 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA5F74 4                       EOR     R4, R4, R12
            R4 = R4 ^ R12;
            //ROM:01BA5F78 4                       STR     R5, [SP,#0x50+var_50]
            put_long((SP + 0x50 - 0x50), R5);
            //ROM:01BA5F7C 4                       SUB     R0, R0, #0x1280000
            R0 = R0 - 0x1280000;
            //ROM:01BA5F80 4                       ADD     R0, R0, #0x70000000
            R0 = R0 + 0x70000000;
            //ROM:01BA5F84 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA5F88 4                       ADD     R4, R5, R0,ROR#27
            R4 = R5 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA5F8C 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA5F90 4                       LDR     R4, [SP,#0x50+var_18]
            R4 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA5F94 4                       LDR     R5, [SP,#0x50+var_2C]
            R5 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA5F98 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA5F9C 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5FA0 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA5FA4 4                       SUB     R2, R2, #0x5F
            R2 = R2 - 0x5F;
            //ROM:01BA5FA8 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA5FAC 4                       LDR     R4, [SP,#0x50+var_4C]
            R4 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA5FB0 4                       ADD     R2, R2, #0x1EC00
            R2 = R2 + 0x1EC00;
            //ROM:01BA5FB4 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA5FB8 4                       EOR     R5, R0, R3
            R5 = R0 ^ R3;
            //ROM:01BA5FBC 4                       MOV     R4, R4,ROR#31
            R4 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA5FC0 4                       EOR     R5, R5, LR
            R5 = R5 ^ LR;
            //ROM:01BA5FC4 4                       SUB     R2, R2, #0x1280000
            R2 = R2 - 0x1280000;
            //ROM:01BA5FC8 4                       ADD     R2, R2, #0x70000000
            R2 = R2 + 0x70000000;
            //ROM:01BA5FCC 4                       ADD     R5, R4, R5
            R5 = R4 + R5;
            //ROM:01BA5FD0 4                       STR     R4, [SP,#0x50+var_4C]
            put_long((SP + 0x50 - 0x4C), R4);
            //ROM:01BA5FD4 4                       ADD     R4, R5, R2,ROR#27
            R4 = R5 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA5FD8 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA5FDC 4                       LDR     R4, [SP,#0x50+var_14]
            R4 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA5FE0 4                       LDR     R5, [SP,#0x50+var_28]
            R5 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA5FE4 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA5FE8 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA5FEC 4                       LDR     R5, [SP,#0x50+var_40]
            R5 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA5FF0 4                       SUB     R12, R12, #0x5F
            R12 = R12 - 0x5F;
            //ROM:01BA5FF4 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA5FF8 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA5FFC 4                       ADD     R12, R12, #0x1EC00
            R12 = R12 + 0x1EC00;
            //ROM:01BA6000 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6004 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6008 4                       EOR     R4, R2, R0
            R4 = R2 ^ R0;
            //ROM:01BA600C 4                       EOR     R4, R4, R3
            R4 = R4 ^ R3;
            //ROM:01BA6010 4                       STR     R5, [SP,#0x50+var_48]
            put_long((SP + 0x50 - 0x48), R5);
            //ROM:01BA6014 4                       SUB     R12, R12, #0x1280000
            R12 = R12 - 0x1280000;
            //ROM:01BA6018 4                       ADD     R12, R12, #0x70000000
            R12 = R12 + 0x70000000;
            //ROM:01BA601C 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA6020 4                       ADD     R4, R5, R12,ROR#27
            R4 = R5 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA6024 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA6028 4                       LDR     R4, [SP,#0x50+var_24]
            R4 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA602C 4                       LDR     R5, [SP,#0x50+var_50]
            R5 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA6030 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA6034 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6038 4                       LDR     R4, [SP,#0x50+var_3C]
            R4 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA603C 4                       SUB     LR, LR, #0x5F
            LR = LR - 0x5F;
            //ROM:01BA6040 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6044 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA6048 4                       ADD     LR, LR, #0x1EC00
            LR = LR + 0x1EC00;
            //ROM:01BA604C 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6050 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6054 4                       EOR     R4, R12, R2
            R4 = R12 ^ R2;
            //ROM:01BA6058 4                       EOR     R4, R4, R0
            R4 = R4 ^ R0;
            //ROM:01BA605C 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6060 4                       SUB     LR, LR, #0x1280000
            LR = LR - 0x1280000;
            //ROM:01BA6064 4                       ADD     LR, LR, #0x70000000
            LR = LR + 0x70000000;
            //ROM:01BA6068 4                       ADD     R4, R4, LR,ROR#27
            R4 = R4 + ((LR >> 27) | (LR << 5));
            //ROM:01BA606C 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA6070 4                       STR     R5, [SP,#0x50+var_44]
            put_long((SP + 0x50 - 0x44), R5);
            //ROM:01BA6074 4                       LDR     R5, [SP,#0x50+var_4C]
            R5 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA6078 4                       LDR     R4, [SP,#0x50+var_20]
            R4 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA607C 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA6080 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6084 4                       LDR     R5, [SP,#0x50+var_38]
            R5 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA6088 4                       SUB     R3, R3, #0x5F
            R3 = R3 - 0x5F;
            //ROM:01BA608C 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6090 4                       LDR     R4, [SP,#0x50+var_40]
            R4 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA6094 4                       ADD     R3, R3, #0x1EC00
            R3 = R3 + 0x1EC00;
            //ROM:01BA6098 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA609C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA60A0 4                       EOR     R4, LR, R12
            R4 = LR ^ R12;
            //ROM:01BA60A4 4                       EOR     R4, R4, R2
            R4 = R4 ^ R2;
            //ROM:01BA60A8 4                       SUB     R3, R3, #0x1280000
            R3 = R3 - 0x1280000;
            //ROM:01BA60AC 4                       ADD     R3, R3, #0x70000000
            R3 = R3 + 0x70000000;
            //ROM:01BA60B0 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA60B4 4                       ADD     R4, R4, R3,ROR#27
            R4 = R4 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA60B8 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA60BC 4                       SUB     R0, R0, #0x5F
            R0 = R0 - 0x5F;
            //ROM:01BA60C0 4                       ADD     R0, R0, #0x1EC00
            R0 = R0 + 0x1EC00;
            //ROM:01BA60C4 4                       SUB     R0, R0, #0x1280000
            R0 = R0 - 0x1280000;
            //ROM:01BA60C8 4                       ADD     R0, R0, #0x70000000
            R0 = R0 + 0x70000000;
            //ROM:01BA60CC 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA60D0 4                       STR     R5, [SP,#0x50+var_40]
            put_long((SP + 0x50 - 0x40), R5);
            //ROM:01BA60D4 4                       LDR     R4, [SP,#0x50+var_1C]
            R4 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA60D8 4                       LDR     R5, [SP,#0x50+var_48]
            R5 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA60DC 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA60E0 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA60E4 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA60E8 4                       LDR     R4, [SP,#0x50+var_3C]
            R4 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA60EC 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA60F0 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA60F4 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA60F8 4                       EOR     R4, R4, R12
            R4 = R4 ^ R12;
            //ROM:01BA60FC 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6100 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA6104 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA6108 4                       STR     R5, [SP,#0x50+var_3C]
            put_long((SP + 0x50 - 0x3C), R5);
            //ROM:01BA610C 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA6110 4                       LDR     R4, [SP,#0x50+var_18]
            R4 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA6114 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA6118 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA611C 4                       LDR     R4, [SP,#0x50+var_30]
            R4 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA6120 4                       SUB     R2, R2, #0x5F
            R2 = R2 - 0x5F;
            //ROM:01BA6124 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6128 4                       LDR     R4, [SP,#0x50+var_38]
            R4 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA612C 4                       ADD     R2, R2, #0x1EC00
            R2 = R2 + 0x1EC00;
            //ROM:01BA6130 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6134 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6138 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA613C 4                       EOR     R4, R4, LR
            R4 = R4 ^ LR;
            //ROM:01BA6140 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6144 4                       SUB     R2, R2, #0x1280000
            R2 = R2 - 0x1280000;
            //ROM:01BA6148 4                       ADD     R2, R2, #0x70000000
            R2 = R2 + 0x70000000;
            //ROM:01BA614C 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA6150 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA6154 4                       STR     R5, [SP,#0x50+var_38]
            put_long((SP + 0x50 - 0x38), R5);
            //ROM:01BA6158 4                       LDR     R5, [SP,#0x50+var_14]
            R5 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA615C 4                       LDR     R4, [SP,#0x50+var_40]
            R4 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA6160 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA6164 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6168 4                       LDR     R4, [SP,#0x50+var_2C]
            R4 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA616C 4                       SUB     R12, R12, #0x5F
            R12 = R12 - 0x5F;
            //ROM:01BA6170 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6174 4                       LDR     R5, [SP,#0x50+var_34]
            R5 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA6178 4                       ADD     R12, R12, #0x1EC00
            R12 = R12 + 0x1EC00;
            //ROM:01BA617C 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6180 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6184 4                       EOR     R4, R2, R0
            R4 = R2 ^ R0;
            //ROM:01BA6188 4                       EOR     R4, R4, R3
            R4 = R4 ^ R3;
            //ROM:01BA618C 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6190 4                       SUB     R12, R12, #0x1280000
            R12 = R12 - 0x1280000;
            //ROM:01BA6194 4                       ADD     R12, R12, #0x70000000
            R12 = R12 + 0x70000000;
            //ROM:01BA6198 4                       ADD     R4, R4, R12,ROR#27
            R4 = R4 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA619C 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA61A0 4                       STR     R5, [SP,#0x50+var_34]
            put_long((SP + 0x50 - 0x34), R5);
            //ROM:01BA61A4 4                       LDR     R5, [SP,#0x50+var_3C]
            R5 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA61A8 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA61AC 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA61B0 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA61B4 4                       LDR     R4, [SP,#0x50+var_28]
            R4 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA61B8 4                       SUB     LR, LR, #0x5F
            LR = LR - 0x5F;
            //ROM:01BA61BC 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA61C0 4                       LDR     R4, [SP,#0x50+var_30]
            R4 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA61C4 4                       ADD     LR, LR, #0x1EC00
            LR = LR + 0x1EC00;
            //ROM:01BA61C8 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA61CC 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA61D0 4                       ORR     R4, R12, R2
            R4 = R12 | R2;
            //ROM:01BA61D4 4                       AND     R5, R12, R2
            R5 = R12 & R2;
            //ROM:01BA61D8 4                       AND     R4, R4, R0
            R4 = R4 & R0;
            //ROM:01BA61DC 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA61E0 4                       SUB     LR, LR, #0x1280000
            LR = LR - 0x1280000;
            //ROM:01BA61E4 4                       ADD     LR, LR, #0x70000000
            LR = LR + 0x70000000;
            //ROM:01BA61E8 4                       ADD     R5, R6, R4
            R5 = R6 + R4;
            //ROM:01BA61EC 4                       ADD     R4, R5, LR,ROR#27
            R4 = R5 + ((LR >> 27) | (LR << 5));
            //ROM:01BA61F0 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA61F4 4                       LDR     R4, [SP,#0x50+var_38]
            R4 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA61F8 4                       LDR     R5, [SP,#0x50+var_4C]
            R5 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA61FC 4                       STR     R6, [SP,#0x50+var_30]
            put_long((SP + 0x50 - 0x30), R6);
            //ROM:01BA6200 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6204 4                       LDR     R4, [SP,#0x50+var_24]
            R4 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA6208 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA620C 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6210 4                       LDR     R5, [SP,#0x50+var_2C]
            R5 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA6214 4                       SUB     R3, R3, #0x324
            R3 = R3 - 0x324;
            //ROM:01BA6218 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA621C 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6220 4                       ORR     R4, LR, R12
            R4 = LR | R12;
            //ROM:01BA6224 4                       AND     R5, LR, R12
            R5 = LR & R12;
            //ROM:01BA6228 4                       AND     R4, R4, R2
            R4 = R4 & R2;
            //ROM:01BA622C 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6230 4                       ADD     R3, R3, #0x1BC000
            R3 = R3 + 0x1BC000;
            //ROM:01BA6234 4                       ADD     R3, R3, #0x8F000000
            R3 = R3 + 0x8F000000;
            //ROM:01BA6238 4                       ADD     R5, R6, R4
            R5 = R6 + R4;
            //ROM:01BA623C 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA6240 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA6244 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA6248 4                       LDR     R5, [SP,#0x50+var_48]
            R5 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA624C 4                       STR     R6, [SP,#0x50+var_2C]
            put_long((SP + 0x50 - 0x2C), R6);
            //ROM:01BA6250 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6254 4                       LDR     R5, [SP,#0x50+var_20]
            R5 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA6258 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA625C 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6260 4                       LDR     R5, [SP,#0x50+var_28]
            R5 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA6264 4                       SUB     R0, R0, #0x324
            R0 = R0 - 0x324;
            //ROM:01BA6268 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA626C 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6270 4                       ORR     R4, R3, LR
            R4 = R3 | LR;
            //ROM:01BA6274 4                       AND     R5, R3, LR
            R5 = R3 & LR;
            //ROM:01BA6278 4                       AND     R4, R4, R12
            R4 = R4 & R12;
            //ROM:01BA627C 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6280 4                       ADD     R0, R0, #0x1BC000
            R0 = R0 + 0x1BC000;
            //ROM:01BA6284 4                       ADD     R0, R0, #0x8F000000
            R0 = R0 + 0x8F000000;
            //ROM:01BA6288 4                       ADD     R5, R6, R4
            R5 = R6 + R4;
            //ROM:01BA628C 4                       ADD     R4, R5, R0,ROR#27
            R4 = R5 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA6290 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA6294 4                       LDR     R4, [SP,#0x50+var_30]
            R4 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA6298 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA629C 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA62A0 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA62A4 4                       LDR     R5, [SP,#0x50+var_1C]
            R5 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA62A8 4                       STR     R6, [SP,#0x50+var_28]
            put_long((SP + 0x50 - 0x28), R6);
            //ROM:01BA62AC 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA62B0 4                       LDR     R5, [SP,#0x50+var_24]
            R5 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA62B4 4                       ORR     R6, R0, R3
            R6 = R0 | R3;
            //ROM:01BA62B8 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA62BC 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA62C0 4                       AND     R4, R0, R3
            R4 = R0 & R3;
            //ROM:01BA62C4 4                       AND     R6, R6, LR
            R6 = R6 & LR;
            //ROM:01BA62C8 4                       SUB     R2, R2, #0x324
            R2 = R2 - 0x324;
            //ROM:01BA62CC 4                       ADD     R2, R2, #0x1BC000
            R2 = R2 + 0x1BC000;
            //ROM:01BA62D0 4                       ORR     R4, R4, R6
            R4 = R4 | R6;
            //ROM:01BA62D4 4                       STR     R5, [SP,#0x50+var_24]
            put_long((SP + 0x50 - 0x24), R5);
            //ROM:01BA62D8 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA62DC 4                       ADD     R2, R2, #0x8F000000
            R2 = R2 + 0x8F000000;
            //ROM:01BA62E0 4                       ADD     R4, R5, R2,ROR#27
            R4 = R5 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA62E4 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA62E8 4                       LDR     R4, [SP,#0x50+var_40]
            R4 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA62EC 4                       LDR     R5, [SP,#0x50+var_2C]
            R5 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA62F0 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA62F4 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA62F8 4                       LDR     R5, [SP,#0x50+var_18]
            R5 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA62FC 4                       SUB     R12, R12, #0x324
            R12 = R12 - 0x324;
            //ROM:01BA6300 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6304 4                       LDR     R5, [SP,#0x50+var_20]
            R5 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA6308 4                       ADD     R12, R12, #0x1BC000
            R12 = R12 + 0x1BC000;
            //ROM:01BA630C 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6310 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6314 4                       ORR     R4, R2, R0
            R4 = R2 | R0;
            //ROM:01BA6318 4                       AND     R5, R2, R0
            R5 = R2 & R0;
            //ROM:01BA631C 4                       AND     R4, R4, R3
            R4 = R4 & R3;
            //ROM:01BA6320 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6324 4                       ADD     R5, R6, R4
            R5 = R6 + R4;
            //ROM:01BA6328 4                       ADD     R12, R12, #0x8F000000
            R12 = R12 + 0x8F000000;
            //ROM:01BA632C 4                       ADD     R4, R5, R12,ROR#27
            R4 = R5 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA6330 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA6334 4                       LDR     R4, [SP,#0x50+var_3C]
            R4 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA6338 4                       LDR     R5, [SP,#0x50+var_28]
            R5 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA633C 4                       STR     R6, [SP,#0x50+var_20]
            put_long((SP + 0x50 - 0x20), R6);
            //ROM:01BA6340 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6344 4                       LDR     R4, [SP,#0x50+var_14]
            R4 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA6348 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA634C 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6350 4                       LDR     R5, [SP,#0x50+var_1C]
            R5 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA6354 4                       SUB     LR, LR, #0x324
            LR = LR - 0x324;
            //ROM:01BA6358 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA635C 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6360 4                       ORR     R4, R12, R2
            R4 = R12 | R2;
            //ROM:01BA6364 4                       AND     R5, R12, R2
            R5 = R12 & R2;
            //ROM:01BA6368 4                       AND     R4, R4, R0
            R4 = R4 & R0;
            //ROM:01BA636C 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6370 4                       ADD     LR, LR, #0x1BC000
            LR = LR + 0x1BC000;
            //ROM:01BA6374 4                       ADD     LR, LR, #0x8F000000
            LR = LR + 0x8F000000;
            //ROM:01BA6378 4                       ADD     R4, R6, R4
            R4 = R6 + R4;
            //ROM:01BA637C 4                       ADD     R4, R4, LR,ROR#27
            R4 = R4 + ((LR >> 27) | (LR << 5));
            //ROM:01BA6380 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA6384 4                       LDR     R4, [SP,#0x50+var_38]
            R4 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA6388 4                       LDR     R5, [SP,#0x50+var_24]
            R5 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA638C 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA6390 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6394 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA6398 4                       STR     R6, [SP,#0x50+var_1C]
            put_long((SP + 0x50 - 0x1C), R6);
            //ROM:01BA639C 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA63A0 4                       LDR     R5, [SP,#0x50+var_18]
            R5 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA63A4 4                       ORR     R6, LR, R12
            R6 = LR | R12;
            //ROM:01BA63A8 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA63AC 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA63B0 4                       AND     R4, LR, R12
            R4 = LR & R12;
            //ROM:01BA63B4 4                       AND     R6, R6, R2
            R6 = R6 & R2;
            //ROM:01BA63B8 4                       SUB     R3, R3, #0x324
            R3 = R3 - 0x324;
            //ROM:01BA63BC 4                       ADD     R3, R3, #0x1BC000
            R3 = R3 + 0x1BC000;
            //ROM:01BA63C0 4                       ORR     R4, R4, R6
            R4 = R4 | R6;
            //ROM:01BA63C4 4                       STR     R5, [SP,#0x50+var_18]
            put_long((SP + 0x50 - 0x18), R5);
            //ROM:01BA63C8 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA63CC 4                       ADD     R3, R3, #0x8F000000
            R3 = R3 + 0x8F000000;
            //ROM:01BA63D0 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA63D4 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA63D8 4                       SUB     R0, R0, #0x324
            R0 = R0 - 0x324;
            //ROM:01BA63DC 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA63E0 4                       LDR     R5, [SP,#0x50+var_20]
            R5 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA63E4 4                       ADD     R0, R0, #0x1BC000
            R0 = R0 + 0x1BC000;
            //ROM:01BA63E8 4                       ADD     R0, R0, #0x8F000000
            R0 = R0 + 0x8F000000;
            //ROM:01BA63EC 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA63F0 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA63F4 4                       LDR     R4, [SP,#0x50+var_4C]
            R4 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA63F8 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA63FC 4                       LDR     R5, [SP,#0x50+var_14]
            R5 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA6400 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6404 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6408 4                       ORR     R4, R3, LR
            R4 = R3 | LR;
            //ROM:01BA640C 4                       AND     R5, R3, LR
            R5 = R3 & LR;
            //ROM:01BA6410 4                       AND     R4, R4, R12
            R4 = R4 & R12;
            //ROM:01BA6414 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6418 4                       ADD     R4, R6, R4
            R4 = R6 + R4;
            //ROM:01BA641C 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA6420 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA6424 4                       LDR     R4, [SP,#0x50+var_1C]
            R4 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA6428 4                       LDR     R5, [SP,#0x50+var_30]
            R5 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA642C 4                       STR     R6, [SP,#0x50+var_14]
            put_long((SP + 0x50 - 0x14), R6);
            //ROM:01BA6430 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6434 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA6438 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA643C 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6440 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA6444 4                       SUB     R2, R2, #0x324
            R2 = R2 - 0x324;
            //ROM:01BA6448 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA644C 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6450 4                       ORR     R4, R0, R3
            R4 = R0 | R3;
            //ROM:01BA6454 4                       AND     R5, R0, R3
            R5 = R0 & R3;
            //ROM:01BA6458 4                       AND     R4, R4, LR
            R4 = R4 & LR;
            //ROM:01BA645C 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6460 4                       ADD     R2, R2, #0x1BC000
            R2 = R2 + 0x1BC000;
            //ROM:01BA6464 4                       ADD     R2, R2, #0x8F000000
            R2 = R2 + 0x8F000000;
            //ROM:01BA6468 4                       ADD     R5, R6, R4
            R5 = R6 + R4;
            //ROM:01BA646C 4                       ADD     R4, R5, R2,ROR#27
            R4 = R5 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA6470 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA6474 4                       LDR     R4, [SP,#0x50+var_2C]
            R4 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA6478 4                       LDR     R5, [SP,#0x50+var_18]
            R5 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA647C 4                       STR     R6, [SP,#0x50+var_50]
            put_long((SP + 0x50 - 0x50), R6);
            //ROM:01BA6480 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6484 4                       LDR     R4, [SP,#0x50+var_44]
            R4 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA6488 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA648C 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6490 4                       LDR     R5, [SP,#0x50+var_4C]
            R5 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA6494 4                       SUB     R12, R12, #0x324
            R12 = R12 - 0x324;
            //ROM:01BA6498 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA649C 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA64A0 4                       ORR     R4, R2, R0
            R4 = R2 | R0;
            //ROM:01BA64A4 4                       AND     R5, R2, R0
            R5 = R2 & R0;
            //ROM:01BA64A8 4                       AND     R4, R4, R3
            R4 = R4 & R3;
            //ROM:01BA64AC 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA64B0 4                       ADD     R12, R12, #0x1BC000
            R12 = R12 + 0x1BC000;
            //ROM:01BA64B4 4                       ADD     R12, R12, #0x8F000000
            R12 = R12 + 0x8F000000;
            //ROM:01BA64B8 4                       ADD     R5, R6, R4
            R5 = R6 + R4;
            //ROM:01BA64BC 4                       ADD     R4, R5, R12,ROR#27
            R4 = R5 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA64C0 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA64C4 4                       LDR     R4, [SP,#0x50+var_28]
            R4 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA64C8 4                       LDR     R5, [SP,#0x50+var_14]
            R5 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA64CC 4                       STR     R6, [SP,#0x50+var_4C]
            put_long((SP + 0x50 - 0x4C), R6);
            //ROM:01BA64D0 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA64D4 4                       LDR     R5, [SP,#0x50+var_40]
            R5 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA64D8 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA64DC 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA64E0 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA64E4 4                       SUB     LR, LR, #0x324
            LR = LR - 0x324;
            //ROM:01BA64E8 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA64EC 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA64F0 4                       ORR     R4, R12, R2
            R4 = R12 | R2;
            //ROM:01BA64F4 4                       AND     R5, R12, R2
            R5 = R12 & R2;
            //ROM:01BA64F8 4                       AND     R4, R4, R0
            R4 = R4 & R0;
            //ROM:01BA64FC 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6500 4                       ADD     LR, LR, #0x1BC000
            LR = LR + 0x1BC000;
            //ROM:01BA6504 4                       ADD     LR, LR, #0x8F000000
            LR = LR + 0x8F000000;
            //ROM:01BA6508 4                       ADD     R5, R6, R4
            R5 = R6 + R4;
            //ROM:01BA650C 4                       ADD     R4, R5, LR,ROR#27
            R4 = R5 + ((LR >> 27) | (LR << 5));
            //ROM:01BA6510 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA6514 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA6518 4                       LDR     R5, [SP,#0x50+var_24]
            R5 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA651C 4                       STR     R6, [SP,#0x50+var_48]
            put_long((SP + 0x50 - 0x48), R6);
            //ROM:01BA6520 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6524 4                       LDR     R5, [SP,#0x50+var_3C]
            R5 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA6528 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA652C 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6530 4                       LDR     R4, [SP,#0x50+var_44]
            R4 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA6534 4                       SUB     R3, R3, #0x324
            R3 = R3 - 0x324;
            //ROM:01BA6538 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA653C 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6540 4                       ORR     R4, LR, R12
            R4 = LR | R12;
            //ROM:01BA6544 4                       AND     R5, LR, R12
            R5 = LR & R12;
            //ROM:01BA6548 4                       AND     R4, R4, R2
            R4 = R4 & R2;
            //ROM:01BA654C 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6550 4                       ADD     R3, R3, #0x1BC000
            R3 = R3 + 0x1BC000;
            //ROM:01BA6554 4                       ADD     R3, R3, #0x8F000000
            R3 = R3 + 0x8F000000;
            //ROM:01BA6558 4                       ADD     R4, R6, R4
            R4 = R6 + R4;
            //ROM:01BA655C 4                       ADD     R4, R4, R3,ROR#27
            R4 = R4 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA6560 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA6564 4                       LDR     R4, [SP,#0x50+var_4C]
            R4 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA6568 4                       LDR     R5, [SP,#0x50+var_20]
            R5 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA656C 4                       STR     R6, [SP,#0x50+var_44]
            put_long((SP + 0x50 - 0x44), R6);
            //ROM:01BA6570 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6574 4                       LDR     R4, [SP,#0x50+var_38]
            R4 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA6578 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA657C 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6580 4                       LDR     R4, [SP,#0x50+var_40]
            R4 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA6584 4                       SUB     R0, R0, #0x324
            R0 = R0 - 0x324;
            //ROM:01BA6588 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA658C 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6590 4                       ORR     R4, R3, LR
            R4 = R3 | LR;
            //ROM:01BA6594 4                       AND     R5, R3, LR
            R5 = R3 & LR;
            //ROM:01BA6598 4                       AND     R4, R4, R12
            R4 = R4 & R12;
            //ROM:01BA659C 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA65A0 4                       ADD     R0, R0, #0x1BC000
            R0 = R0 + 0x1BC000;
            //ROM:01BA65A4 4                       ADD     R0, R0, #0x8F000000
            R0 = R0 + 0x8F000000;
            //ROM:01BA65A8 4                       ADD     R4, R6, R4
            R4 = R6 + R4;
            //ROM:01BA65AC 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA65B0 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA65B4 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA65B8 4                       LDR     R5, [SP,#0x50+var_1C]
            R5 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA65BC 4                       STR     R6, [SP,#0x50+var_40]
            put_long((SP + 0x50 - 0x40), R6);
            //ROM:01BA65C0 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA65C4 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA65C8 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA65CC 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA65D0 4                       LDR     R5, [SP,#0x50+var_3C]
            R5 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA65D4 4                       SUB     R2, R2, #0x324
            R2 = R2 - 0x324;
            //ROM:01BA65D8 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA65DC 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA65E0 4                       ORR     R4, R0, R3
            R4 = R0 | R3;
            //ROM:01BA65E4 4                       AND     R5, R0, R3
            R5 = R0 & R3;
            //ROM:01BA65E8 4                       AND     R4, R4, LR
            R4 = R4 & LR;
            //ROM:01BA65EC 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA65F0 4                       ADD     R2, R2, #0x1BC000
            R2 = R2 + 0x1BC000;
            //ROM:01BA65F4 4                       ADD     R2, R2, #0x8F000000
            R2 = R2 + 0x8F000000;
            //ROM:01BA65F8 4                       ADD     R5, R6, R4
            R5 = R6 + R4;
            //ROM:01BA65FC 4                       ADD     R4, R5, R2,ROR#27
            R4 = R5 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA6600 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA6604 4                       LDR     R4, [SP,#0x50+var_44]
            R4 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA6608 4                       LDR     R5, [SP,#0x50+var_18]
            R5 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA660C 4                       STR     R6, [SP,#0x50+var_3C]
            put_long((SP + 0x50 - 0x3C), R6);
            //ROM:01BA6610 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6614 4                       LDR     R4, [SP,#0x50+var_30]
            R4 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA6618 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA661C 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6620 4                       LDR     R4, [SP,#0x50+var_38]
            R4 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA6624 4                       SUB     R12, R12, #0x324
            R12 = R12 - 0x324;
            //ROM:01BA6628 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA662C 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6630 4                       ORR     R4, R2, R0
            R4 = R2 | R0;
            //ROM:01BA6634 4                       AND     R5, R2, R0
            R5 = R2 & R0;
            //ROM:01BA6638 4                       AND     R4, R4, R3
            R4 = R4 & R3;
            //ROM:01BA663C 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6640 4                       ADD     R12, R12, #0x1BC000
            R12 = R12 + 0x1BC000;
            //ROM:01BA6644 4                       ADD     R12, R12, #0x8F000000
            R12 = R12 + 0x8F000000;
            //ROM:01BA6648 4                       ADD     R4, R6, R4
            R4 = R6 + R4;
            //ROM:01BA664C 4                       ADD     R4, R4, R12,ROR#27
            R4 = R4 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA6650 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA6654 4                       LDR     R4, [SP,#0x50+var_14]
            R4 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA6658 4                       LDR     R5, [SP,#0x50+var_40]
            R5 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA665C 4                       STR     R6, [SP,#0x50+var_38]
            put_long((SP + 0x50 - 0x38), R6);
            //ROM:01BA6660 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6664 4                       LDR     R4, [SP,#0x50+var_2C]
            R4 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA6668 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA666C 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6670 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA6674 4                       SUB     LR, LR, #0x324
            LR = LR - 0x324;
            //ROM:01BA6678 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA667C 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6680 4                       ORR     R4, R12, R2
            R4 = R12 | R2;
            //ROM:01BA6684 4                       AND     R5, R12, R2
            R5 = R12 & R2;
            //ROM:01BA6688 4                       AND     R4, R4, R0
            R4 = R4 & R0;
            //ROM:01BA668C 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6690 4                       ADD     LR, LR, #0x1BC000
            LR = LR + 0x1BC000;
            //ROM:01BA6694 4                       ADD     LR, LR, #0x8F000000
            LR = LR + 0x8F000000;
            //ROM:01BA6698 4                       ADD     R5, R6, R4
            R5 = R6 + R4;
            //ROM:01BA669C 4                       ADD     R4, R5, LR,ROR#27
            R4 = R5 + ((LR >> 27) | (LR << 5));
            //ROM:01BA66A0 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA66A4 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA66A8 4                       LDR     R5, [SP,#0x50+var_3C]
            R5 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA66AC 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA66B0 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA66B4 4                       LDR     R4, [SP,#0x50+var_28]
            R4 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA66B8 4                       STR     R6, [SP,#0x50+var_34]
            put_long((SP + 0x50 - 0x34), R6);
            //ROM:01BA66BC 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA66C0 4                       LDR     R5, [SP,#0x50+var_30]
            R5 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA66C4 4                       ORR     R6, LR, R12
            R6 = LR | R12;
            //ROM:01BA66C8 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA66CC 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA66D0 4                       AND     R4, LR, R12
            R4 = LR & R12;
            //ROM:01BA66D4 4                       AND     R6, R6, R2
            R6 = R6 & R2;
            //ROM:01BA66D8 4                       SUB     R3, R3, #0x324
            R3 = R3 - 0x324;
            //ROM:01BA66DC 4                       ADD     R3, R3, #0x1BC000
            R3 = R3 + 0x1BC000;
            //ROM:01BA66E0 4                       ORR     R4, R4, R6
            R4 = R4 | R6;
            //ROM:01BA66E4 4                       STR     R5, [SP,#0x50+var_30]
            put_long((SP + 0x50 - 0x30), R5);
            //ROM:01BA66E8 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA66EC 4                       ADD     R3, R3, #0x8F000000
            R3 = R3 + 0x8F000000;
            //ROM:01BA66F0 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA66F4 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA66F8 4                       SUB     R0, R0, #0x324
            R0 = R0 - 0x324;
            //ROM:01BA66FC 4                       LDR     R4, [SP,#0x50+var_38]
            R4 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA6700 4                       LDR     R5, [SP,#0x50+var_4C]
            R5 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA6704 4                       ADD     R0, R0, #0x1BC000
            R0 = R0 + 0x1BC000;
            //ROM:01BA6708 4                       ADD     R0, R0, #0x8F000000
            R0 = R0 + 0x8F000000;
            //ROM:01BA670C 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA6710 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6714 4                       LDR     R5, [SP,#0x50+var_24]
            R5 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA6718 4                       ORR     R6, R3, LR
            R6 = R3 | LR;
            //ROM:01BA671C 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6720 4                       LDR     R4, [SP,#0x50+var_2C]
            R4 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA6724 4                       AND     R6, R6, R12
            R6 = R6 & R12;
            //ROM:01BA6728 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA672C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6730 4                       AND     R4, R3, LR
            R4 = R3 & LR;
            //ROM:01BA6734 4                       ORR     R4, R4, R6
            R4 = R4 | R6;
            //ROM:01BA6738 4                       STR     R5, [SP,#0x50+var_2C]
            put_long((SP + 0x50 - 0x2C), R5);
            //ROM:01BA673C 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA6740 4                       ADD     R4, R5, R0,ROR#27
            R4 = R5 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA6744 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA6748 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA674C 4                       LDR     R5, [SP,#0x50+var_48]
            R5 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA6750 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA6754 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6758 4                       LDR     R4, [SP,#0x50+var_20]
            R4 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA675C 4                       SUB     R2, R2, #0x324
            R2 = R2 - 0x324;
            //ROM:01BA6760 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6764 4                       LDR     R5, [SP,#0x50+var_28]
            R5 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA6768 4                       ADD     R2, R2, #0x1BC000
            R2 = R2 + 0x1BC000;
            //ROM:01BA676C 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6770 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6774 4                       ORR     R4, R0, R3
            R4 = R0 | R3;
            //ROM:01BA6778 4                       AND     R5, R0, R3
            R5 = R0 & R3;
            //ROM:01BA677C 4                       AND     R4, R4, LR
            R4 = R4 & LR;
            //ROM:01BA6780 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA6784 4                       ADD     R4, R6, R4
            R4 = R6 + R4;
            //ROM:01BA6788 4                       ADD     R2, R2, #0x8F000000
            R2 = R2 + 0x8F000000;
            //ROM:01BA678C 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA6790 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA6794 4                       LDR     R4, [SP,#0x50+var_30]
            R4 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA6798 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA679C 4                       STR     R6, [SP,#0x50+var_28]
            put_long((SP + 0x50 - 0x28), R6);
            //ROM:01BA67A0 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA67A4 4                       LDR     R5, [SP,#0x50+var_1C]
            R5 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA67A8 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA67AC 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA67B0 4                       LDR     R4, [SP,#0x50+var_24]
            R4 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA67B4 4                       SUB     R12, R12, #0x324
            R12 = R12 - 0x324;
            //ROM:01BA67B8 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA67BC 4                       MOV     R6, R4,ROR#31
            R6 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA67C0 4                       ORR     R4, R2, R0
            R4 = R2 | R0;
            //ROM:01BA67C4 4                       AND     R5, R2, R0
            R5 = R2 & R0;
            //ROM:01BA67C8 4                       AND     R4, R4, R3
            R4 = R4 & R3;
            //ROM:01BA67CC 4                       ORR     R4, R5, R4
            R4 = R5 | R4;
            //ROM:01BA67D0 4                       ADD     R12, R12, #0x1BC000
            R12 = R12 + 0x1BC000;
            //ROM:01BA67D4 4                       ADD     R12, R12, #0x8F000000
            R12 = R12 + 0x8F000000;
            //ROM:01BA67D8 4                       ADD     R5, R6, R4
            R5 = R6 + R4;
            //ROM:01BA67DC 4                       ADD     R4, R5, R12,ROR#27
            R4 = R5 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA67E0 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA67E4 4                       LDR     R4, [SP,#0x50+var_40]
            R4 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA67E8 4                       LDR     R5, [SP,#0x50+var_2C]
            R5 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA67EC 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA67F0 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA67F4 4                       LDR     R4, [SP,#0x50+var_18]
            R4 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA67F8 4                       SUB     LR, LR, #0x324
            LR = LR - 0x324;
            //ROM:01BA67FC 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6800 4                       LDR     R4, [SP,#0x50+var_20]
            R4 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA6804 4                       ADD     LR, LR, #0x1BC000
            LR = LR + 0x1BC000;
            //ROM:01BA6808 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA680C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6810 4                       EOR     R4, R12, R2
            R4 = R12 ^ R2;
            //ROM:01BA6814 4                       EOR     R4, R4, R0
            R4 = R4 ^ R0;
            //ROM:01BA6818 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA681C 4                       ADD     LR, LR, #0x8F000000
            LR = LR + 0x8F000000;
            //ROM:01BA6820 4                       ADD     R4, R4, LR,ROR#27
            R4 = R4 + ((LR >> 27) | (LR << 5));
            //ROM:01BA6824 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA6828 4                       STR     R5, [SP,#0x50+var_20]
            put_long((SP + 0x50 - 0x20), R5);
            //ROM:01BA682C 4                       LDR     R5, [SP,#0x50+var_28]
            R5 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA6830 4                       LDR     R4, [SP,#0x50+var_3C]
            R4 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA6834 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA6838 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA683C 4                       LDR     R5, [SP,#0x50+var_14]
            R5 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA6840 4                       ADD     R3, R3, #0xD6
            R3 = R3 + 0xD6;
            //ROM:01BA6844 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6848 4                       LDR     R4, [SP,#0x50+var_1C]
            R4 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA684C 4                       ADD     R3, R3, #0xC100
            R3 = R3 + 0xC100;
            //ROM:01BA6850 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6854 4                       EOR     R5, LR, R12
            R5 = LR ^ R12;
            //ROM:01BA6858 4                       MOV     R4, R4,ROR#31
            R4 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA685C 4                       STR     R4, [SP,#0x50+var_1C]
            put_long((SP + 0x50 - 0x1C), R4);
            //ROM:01BA6860 4                       EOR     R5, R5, R2
            R5 = R5 ^ R2;
            //ROM:01BA6864 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01BA6868 4                       ADD     R3, R3, #0x620000
            R3 = R3 + 0x620000;
            //ROM:01BA686C 4                       ADD     R3, R3, #0xCA000000
            R3 = R3 + 0xCA000000;
            //ROM:01BA6870 4                       ADD     R4, R4, R3,ROR#27
            R4 = R4 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA6874 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA6878 4                       STR     R6, [SP,#0x50+var_24]
            put_long((SP + 0x50 - 0x24), R6);
            //ROM:01BA687C 4                       LDR     R4, [SP,#0x50+var_24]
            R4 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA6880 4                       LDR     R5, [SP,#0x50+var_38]
            R5 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA6884 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA6888 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA688C 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA6890 4                       ADD     R0, R0, #0xD6
            R0 = R0 + 0xD6;
            //ROM:01BA6894 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6898 4                       LDR     R4, [SP,#0x50+var_18]
            R4 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA689C 4                       ADD     R0, R0, #0xC100
            R0 = R0 + 0xC100;
            //ROM:01BA68A0 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA68A4 4                       EOR     R5, R3, LR
            R5 = R3 ^ LR;
            //ROM:01BA68A8 4                       MOV     R4, R4,ROR#31
            R4 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA68AC 4                       STR     R4, [SP,#0x50+var_18]
            put_long((SP + 0x50 - 0x18), R4);
            //ROM:01BA68B0 4                       EOR     R5, R5, R12
            R5 = R5 ^ R12;
            //ROM:01BA68B4 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01BA68B8 4                       ADD     R0, R0, #0x620000
            R0 = R0 + 0x620000;
            //ROM:01BA68BC 4                       ADD     R0, R0, #0xCA000000
            R0 = R0 + 0xCA000000;
            //ROM:01BA68C0 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA68C4 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA68C8 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA68CC 4                       LDR     R5, [SP,#0x50+var_20]
            R5 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA68D0 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA68D4 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA68D8 4                       LDR     R5, [SP,#0x50+var_4C]
            R5 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA68DC 4                       ADD     R2, R2, #0xD6
            R2 = R2 + 0xD6;
            //ROM:01BA68E0 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA68E4 4                       LDR     R5, [SP,#0x50+var_14]
            R5 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA68E8 4                       ADD     R2, R2, #0xC100
            R2 = R2 + 0xC100;
            //ROM:01BA68EC 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA68F0 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA68F4 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA68F8 4                       EOR     R4, R4, LR
            R4 = R4 ^ LR;
            //ROM:01BA68FC 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6900 4                       ADD     R2, R2, #0x620000
            R2 = R2 + 0x620000;
            //ROM:01BA6904 4                       ADD     R2, R2, #0xCA000000
            R2 = R2 + 0xCA000000;
            //ROM:01BA6908 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA690C 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA6910 4                       STR     R5, [SP,#0x50+var_14]
            put_long((SP + 0x50 - 0x14), R5);
            //ROM:01BA6914 4                       LDR     R5, [SP,#0x50+var_30]
            R5 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA6918 4                       LDR     R4, [SP,#0x50+var_1C]
            R4 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA691C 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA6920 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6924 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA6928 4                       ADD     R12, R12, #0xD6
            R12 = R12 + 0xD6;
            //ROM:01BA692C 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6930 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA6934 4                       ADD     R12, R12, #0xC100
            R12 = R12 + 0xC100;
            //ROM:01BA6938 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA693C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6940 4                       EOR     R4, R2, R0
            R4 = R2 ^ R0;
            //ROM:01BA6944 4                       EOR     R4, R4, R3
            R4 = R4 ^ R3;
            //ROM:01BA6948 4                       STR     R5, [SP,#0x50+var_50]
            put_long((SP + 0x50 - 0x50), R5);
            //ROM:01BA694C 4                       ADD     R12, R12, #0x620000
            R12 = R12 + 0x620000;
            //ROM:01BA6950 4                       ADD     R12, R12, #0xCA000000
            R12 = R12 + 0xCA000000;
            //ROM:01BA6954 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA6958 4                       ADD     R4, R5, R12,ROR#27
            R4 = R5 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA695C 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA6960 4                       LDR     R4, [SP,#0x50+var_18]
            R4 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA6964 4                       LDR     R5, [SP,#0x50+var_2C]
            R5 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA6968 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA696C 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6970 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA6974 4                       ADD     LR, LR, #0xD6
            LR = LR + 0xD6;
            //ROM:01BA6978 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA697C 4                       LDR     R5, [SP,#0x50+var_4C]
            R5 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA6980 4                       ADD     LR, LR, #0xC100
            LR = LR + 0xC100;
            //ROM:01BA6984 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6988 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA698C 4                       EOR     R4, R12, R2
            R4 = R12 ^ R2;
            //ROM:01BA6990 4                       EOR     R4, R4, R0
            R4 = R4 ^ R0;
            //ROM:01BA6994 4                       STR     R5, [SP,#0x50+var_4C]
            put_long((SP + 0x50 - 0x4C), R5);
            //ROM:01BA6998 4                       ADD     LR, LR, #0x620000
            LR = LR + 0x620000;
            //ROM:01BA699C 4                       ADD     LR, LR, #0xCA000000
            LR = LR + 0xCA000000;
            //ROM:01BA69A0 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA69A4 4                       ADD     R4, R5, LR,ROR#27
            R4 = R5 + ((LR >> 27) | (LR << 5));
            //ROM:01BA69A8 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA69AC 4                       LDR     R4, [SP,#0x50+var_14]
            R4 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA69B0 4                       LDR     R5, [SP,#0x50+var_28]
            R5 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA69B4 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA69B8 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA69BC 4                       LDR     R4, [SP,#0x50+var_40]
            R4 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA69C0 4                       ADD     R3, R3, #0xD6
            R3 = R3 + 0xD6;
            //ROM:01BA69C4 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA69C8 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA69CC 4                       ADD     R3, R3, #0xC100
            R3 = R3 + 0xC100;
            //ROM:01BA69D0 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA69D4 4                       EOR     R5, LR, R12
            R5 = LR ^ R12;
            //ROM:01BA69D8 4                       MOV     R4, R4,ROR#31
            R4 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA69DC 4                       STR     R4, [SP,#0x50+var_48]
            put_long((SP + 0x50 - 0x48), R4);
            //ROM:01BA69E0 4                       EOR     R5, R5, R2
            R5 = R5 ^ R2;
            //ROM:01BA69E4 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01BA69E8 4                       ADD     R3, R3, #0x620000
            R3 = R3 + 0x620000;
            //ROM:01BA69EC 4                       ADD     R3, R3, #0xCA000000
            R3 = R3 + 0xCA000000;
            //ROM:01BA69F0 4                       ADD     R4, R4, R3,ROR#27
            R4 = R4 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA69F4 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA69F8 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA69FC 4                       LDR     R5, [SP,#0x50+var_24]
            R5 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA6A00 4                       ADD     R0, R0, #0xD6
            R0 = R0 + 0xD6;
            //ROM:01BA6A04 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6A08 4                       LDR     R5, [SP,#0x50+var_3C]
            R5 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA6A0C 4                       ADD     R0, R0, #0xC100
            R0 = R0 + 0xC100;
            //ROM:01BA6A10 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6A14 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA6A18 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA6A1C 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6A20 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6A24 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA6A28 4                       ADD     R0, R0, #0x620000
            R0 = R0 + 0x620000;
            //ROM:01BA6A2C 4                       ADD     R0, R0, #0xCA000000
            R0 = R0 + 0xCA000000;
            //ROM:01BA6A30 4                       EOR     R4, R4, R12
            R4 = R4 ^ R12;
            //ROM:01BA6A34 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6A38 4                       ADD     R4, R4, R0,ROR#27
            R4 = R4 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA6A3C 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA6A40 4                       STR     R5, [SP,#0x50+var_44]
            put_long((SP + 0x50 - 0x44), R5);
            //ROM:01BA6A44 4                       LDR     R5, [SP,#0x50+var_4C]
            R5 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA6A48 4                       LDR     R4, [SP,#0x50+var_20]
            R4 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA6A4C 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA6A50 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6A54 4                       LDR     R5, [SP,#0x50+var_38]
            R5 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA6A58 4                       ADD     R2, R2, #0xD6
            R2 = R2 + 0xD6;
            //ROM:01BA6A5C 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6A60 4                       LDR     R5, [SP,#0x50+var_40]
            R5 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA6A64 4                       ADD     R2, R2, #0xC100
            R2 = R2 + 0xC100;
            //ROM:01BA6A68 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6A6C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6A70 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA6A74 4                       EOR     R4, R4, LR
            R4 = R4 ^ LR;
            //ROM:01BA6A78 4                       STR     R5, [SP,#0x50+var_40]
            put_long((SP + 0x50 - 0x40), R5);
            //ROM:01BA6A7C 4                       ADD     R2, R2, #0x620000
            R2 = R2 + 0x620000;
            //ROM:01BA6A80 4                       ADD     R2, R2, #0xCA000000
            R2 = R2 + 0xCA000000;
            //ROM:01BA6A84 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA6A88 4                       ADD     R4, R5, R2,ROR#27
            R4 = R5 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA6A8C 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA6A90 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA6A94 4                       LDR     R5, [SP,#0x50+var_1C]
            R5 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA6A98 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA6A9C 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6AA0 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA6AA4 4                       ADD     R12, R12, #0xD6
            R12 = R12 + 0xD6;
            //ROM:01BA6AA8 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6AAC 4                       LDR     R5, [SP,#0x50+var_3C]
            R5 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA6AB0 4                       ADD     R12, R12, #0xC100
            R12 = R12 + 0xC100;
            //ROM:01BA6AB4 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6AB8 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6ABC 4                       EOR     R4, R2, R0
            R4 = R2 ^ R0;
            //ROM:01BA6AC0 4                       EOR     R4, R4, R3
            R4 = R4 ^ R3;
            //ROM:01BA6AC4 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6AC8 4                       ADD     R12, R12, #0x620000
            R12 = R12 + 0x620000;
            //ROM:01BA6ACC 4                       ADD     R12, R12, #0xCA000000
            R12 = R12 + 0xCA000000;
            //ROM:01BA6AD0 4                       ADD     R4, R4, R12,ROR#27
            R4 = R4 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA6AD4 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA6AD8 4                       STR     R5, [SP,#0x50+var_3C]
            put_long((SP + 0x50 - 0x3C), R5);
            //ROM:01BA6ADC 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA6AE0 4                       LDR     R4, [SP,#0x50+var_18]
            R4 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA6AE4 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA6AE8 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6AEC 4                       LDR     R5, [SP,#0x50+var_30]
            R5 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA6AF0 4                       ADD     LR, LR, #0xD6
            LR = LR + 0xD6;
            //ROM:01BA6AF4 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6AF8 4                       LDR     R4, [SP,#0x50+var_38]
            R4 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA6AFC 4                       ADD     LR, LR, #0xC100
            LR = LR + 0xC100;
            //ROM:01BA6B00 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6B04 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6B08 4                       EOR     R4, R12, R2
            R4 = R12 ^ R2;
            //ROM:01BA6B0C 4                       EOR     R4, R4, R0
            R4 = R4 ^ R0;
            //ROM:01BA6B10 4                       STR     R5, [SP,#0x50+var_38]
            put_long((SP + 0x50 - 0x38), R5);
            //ROM:01BA6B14 4                       ADD     LR, LR, #0x620000
            LR = LR + 0x620000;
            //ROM:01BA6B18 4                       ADD     LR, LR, #0xCA000000
            LR = LR + 0xCA000000;
            //ROM:01BA6B1C 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA6B20 4                       ADD     R4, R5, LR,ROR#27
            R4 = R5 + ((LR >> 27) | (LR << 5));
            //ROM:01BA6B24 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA6B28 4                       LDR     R4, [SP,#0x50+var_40]
            R4 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA6B2C 4                       LDR     R5, [SP,#0x50+var_14]
            R5 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA6B30 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA6B34 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6B38 4                       LDR     R5, [SP,#0x50+var_2C]
            R5 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA6B3C 4                       ADD     R3, R3, #0xD6
            R3 = R3 + 0xD6;
            //ROM:01BA6B40 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6B44 4                       LDR     R4, [SP,#0x50+var_34]
            R4 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA6B48 4                       ADD     R3, R3, #0xC100
            R3 = R3 + 0xC100;
            //ROM:01BA6B4C 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6B50 4                       EOR     R5, LR, R12
            R5 = LR ^ R12;
            //ROM:01BA6B54 4                       MOV     R4, R4,ROR#31
            R4 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6B58 4                       EOR     R5, R5, R2
            R5 = R5 ^ R2;
            //ROM:01BA6B5C 4                       ADD     R3, R3, #0x620000
            R3 = R3 + 0x620000;
            //ROM:01BA6B60 4                       ADD     R3, R3, #0xCA000000
            R3 = R3 + 0xCA000000;
            //ROM:01BA6B64 4                       ADD     R5, R4, R5
            R5 = R4 + R5;
            //ROM:01BA6B68 4                       STR     R4, [SP,#0x50+var_34]
            put_long((SP + 0x50 - 0x34), R4);
            //ROM:01BA6B6C 4                       ADD     R4, R5, R3,ROR#27
            R4 = R5 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA6B70 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA6B74 4                       LDR     R4, [SP,#0x50+var_3C]
            R4 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA6B78 4                       LDR     R5, [SP,#0x50+var_50]
            R5 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA6B7C 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA6B80 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6B84 4                       LDR     R4, [SP,#0x50+var_28]
            R4 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA6B88 4                       ADD     R0, R0, #0xD6
            R0 = R0 + 0xD6;
            //ROM:01BA6B8C 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6B90 4                       LDR     R5, [SP,#0x50+var_30]
            R5 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA6B94 4                       ADD     R0, R0, #0xC100
            R0 = R0 + 0xC100;
            //ROM:01BA6B98 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6B9C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6BA0 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA6BA4 4                       EOR     R4, R4, R12
            R4 = R4 ^ R12;
            //ROM:01BA6BA8 4                       STR     R5, [SP,#0x50+var_30]
            put_long((SP + 0x50 - 0x30), R5);
            //ROM:01BA6BAC 4                       ADD     R0, R0, #0x620000
            R0 = R0 + 0x620000;
            //ROM:01BA6BB0 4                       ADD     R0, R0, #0xCA000000
            R0 = R0 + 0xCA000000;
            //ROM:01BA6BB4 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA6BB8 4                       ADD     R4, R5, R0,ROR#27
            R4 = R5 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA6BBC 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA6BC0 4                       LDR     R4, [SP,#0x50+var_4C]
            R4 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA6BC4 4                       LDR     R5, [SP,#0x50+var_38]
            R5 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA6BC8 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA6BCC 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6BD0 4                       LDR     R5, [SP,#0x50+var_24]
            R5 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA6BD4 4                       ADD     R2, R2, #0xD6
            R2 = R2 + 0xD6;
            //ROM:01BA6BD8 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6BDC 4                       LDR     R5, [SP,#0x50+var_2C]
            R5 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA6BE0 4                       ADD     R2, R2, #0xC100
            R2 = R2 + 0xC100;
            //ROM:01BA6BE4 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6BE8 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6BEC 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA6BF0 4                       EOR     R4, R4, LR
            R4 = R4 ^ LR;
            //ROM:01BA6BF4 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6BF8 4                       ADD     R2, R2, #0x620000
            R2 = R2 + 0x620000;
            //ROM:01BA6BFC 4                       ADD     R2, R2, #0xCA000000
            R2 = R2 + 0xCA000000;
            //ROM:01BA6C00 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA6C04 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA6C08 4                       STR     R5, [SP,#0x50+var_2C]
            put_long((SP + 0x50 - 0x2C), R5);
            //ROM:01BA6C0C 4                       LDR     R5, [SP,#0x50+var_34]
            R5 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA6C10 4                       LDR     R4, [SP,#0x50+var_48]
            R4 = get_long((SP + 0x50 - 0x48));
            //ROM:01BA6C14 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA6C18 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6C1C 4                       LDR     R5, [SP,#0x50+var_20]
            R5 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA6C20 4                       ADD     R12, R12, #0xD6
            R12 = R12 + 0xD6;
            //ROM:01BA6C24 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6C28 4                       LDR     R4, [SP,#0x50+var_28]
            R4 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA6C2C 4                       ADD     R12, R12, #0xC100
            R12 = R12 + 0xC100;
            //ROM:01BA6C30 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6C34 4                       EOR     R5, R2, R0
            R5 = R2 ^ R0;
            //ROM:01BA6C38 4                       MOV     R4, R4,ROR#31
            R4 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6C3C 4                       EOR     R5, R5, R3
            R5 = R5 ^ R3;
            //ROM:01BA6C40 4                       ADD     R12, R12, #0x620000
            R12 = R12 + 0x620000;
            //ROM:01BA6C44 4                       ADD     R12, R12, #0xCA000000
            R12 = R12 + 0xCA000000;
            //ROM:01BA6C48 4                       ADD     R5, R4, R5
            R5 = R4 + R5;
            //ROM:01BA6C4C 4                       STR     R4, [SP,#0x50+var_28]
            put_long((SP + 0x50 - 0x28), R4);
            //ROM:01BA6C50 4                       ADD     R4, R5, R12,ROR#27
            R4 = R5 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA6C54 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA6C58 4                       LDR     R4, [SP,#0x50+var_30]
            R4 = get_long((SP + 0x50 - 0x30));
            //ROM:01BA6C5C 4                       LDR     R5, [SP,#0x50+var_44]
            R5 = get_long((SP + 0x50 - 0x44));
            //ROM:01BA6C60 4                       MOV     R2, R2,ROR#2
            R2 = ((R2 >> 2) | (R2 << 30));
            //ROM:01BA6C64 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6C68 4                       LDR     R4, [SP,#0x50+var_1C]
            R4 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA6C6C 4                       ADD     LR, LR, #0xD6
            LR = LR + 0xD6;
            //ROM:01BA6C70 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6C74 4                       LDR     R4, [SP,#0x50+var_24]
            R4 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA6C78 4                       ADD     LR, LR, #0xC100
            LR = LR + 0xC100;
            //ROM:01BA6C7C 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6C80 4                       EOR     R5, R12, R2
            R5 = R12 ^ R2;
            //ROM:01BA6C84 4                       MOV     R4, R4,ROR#31
            R4 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6C88 4                       EOR     R5, R5, R0
            R5 = R5 ^ R0;
            //ROM:01BA6C8C 4                       ADD     LR, LR, #0x620000
            LR = LR + 0x620000;
            //ROM:01BA6C90 4                       ADD     LR, LR, #0xCA000000
            LR = LR + 0xCA000000;
            //ROM:01BA6C94 4                       ADD     R5, R4, R5
            R5 = R4 + R5;
            //ROM:01BA6C98 4                       STR     R4, [SP,#0x50+var_24]
            put_long((SP + 0x50 - 0x24), R4);
            //ROM:01BA6C9C 4                       ADD     R4, R5, LR,ROR#27
            R4 = R5 + ((LR >> 27) | (LR << 5));
            //ROM:01BA6CA0 4                       ADD     R3, R4, R3
            R3 = R4 + R3;
            //ROM:01BA6CA4 4                       LDR     R4, [SP,#0x50+var_40]
            R4 = get_long((SP + 0x50 - 0x40));
            //ROM:01BA6CA8 4                       LDR     R5, [SP,#0x50+var_2C]
            R5 = get_long((SP + 0x50 - 0x2C));
            //ROM:01BA6CAC 4                       MOV     R12, R12,ROR#2
            R12 = ((R12 >> 2) | (R12 << 30));
            //ROM:01BA6CB0 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6CB4 4                       LDR     R4, [SP,#0x50+var_18]
            R4 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA6CB8 4                       ADD     R3, R3, #0xD6
            R3 = R3 + 0xD6;
            //ROM:01BA6CBC 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6CC0 4                       LDR     R5, [SP,#0x50+var_20]
            R5 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA6CC4 4                       ADD     R3, R3, #0xC100
            R3 = R3 + 0xC100;
            //ROM:01BA6CC8 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6CCC 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6CD0 4                       EOR     R4, LR, R12
            R4 = LR ^ R12;
            //ROM:01BA6CD4 4                       EOR     R4, R4, R2
            R4 = R4 ^ R2;
            //ROM:01BA6CD8 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6CDC 4                       ADD     R3, R3, #0x620000
            R3 = R3 + 0x620000;
            //ROM:01BA6CE0 4                       ADD     R3, R3, #0xCA000000
            R3 = R3 + 0xCA000000;
            //ROM:01BA6CE4 4                       ADD     R4, R4, R3,ROR#27
            R4 = R4 + ((R3 >> 27) | (R3 << 5));
            //ROM:01BA6CE8 4                       ADD     R0, R4, R0
            R0 = R4 + R0;
            //ROM:01BA6CEC 4                       STR     R5, [SP,#0x50+var_20]
            put_long((SP + 0x50 - 0x20), R5);
            //ROM:01BA6CF0 4                       LDR     R5, [SP,#0x50+var_3C]
            R5 = get_long((SP + 0x50 - 0x3C));
            //ROM:01BA6CF4 4                       LDR     R4, [SP,#0x50+var_28]
            R4 = get_long((SP + 0x50 - 0x28));
            //ROM:01BA6CF8 4                       MOV     LR, LR,ROR#2
            LR = ((LR >> 2) | (LR << 30));
            //ROM:01BA6CFC 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6D00 4                       LDR     R4, [SP,#0x50+var_14]
            R4 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA6D04 4                       ADD     R0, R0, #0xD6
            R0 = R0 + 0xD6;
            //ROM:01BA6D08 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6D0C 4                       LDR     R5, [SP,#0x50+var_1C]
            R5 = get_long((SP + 0x50 - 0x1C));
            //ROM:01BA6D10 4                       ADD     R0, R0, #0xC100
            R0 = R0 + 0xC100;
            //ROM:01BA6D14 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01BA6D18 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6D1C 4                       EOR     R4, R3, LR
            R4 = R3 ^ LR;
            //ROM:01BA6D20 4                       EOR     R4, R4, R12
            R4 = R4 ^ R12;
            //ROM:01BA6D24 4                       STR     R5, [SP,#0x50+var_1C]
            put_long((SP + 0x50 - 0x1C), R5);
            //ROM:01BA6D28 4                       ADD     R0, R0, #0x620000
            R0 = R0 + 0x620000;
            //ROM:01BA6D2C 4                       ADD     R0, R0, #0xCA000000
            R0 = R0 + 0xCA000000;
            //ROM:01BA6D30 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01BA6D34 4                       ADD     R4, R5, R0,ROR#27
            R4 = R5 + ((R0 >> 27) | (R0 << 5));
            //ROM:01BA6D38 4                       ADD     R2, R4, R2
            R2 = R4 + R2;
            //ROM:01BA6D3C 4                       ADD     R2, R2, #0xD6
            R2 = R2 + 0xD6;
            //ROM:01BA6D40 4                       ADD     R2, R2, #0xC100
            R2 = R2 + 0xC100;
            //ROM:01BA6D44 4                       ADD     R2, R2, #0x620000
            R2 = R2 + 0x620000;
            //ROM:01BA6D48 4                       LDR     R4, [SP,#0x50+var_24]
            R4 = get_long((SP + 0x50 - 0x24));
            //ROM:01BA6D4C 4                       ADD     R2, R2, #0xCA000000
            R2 = R2 + 0xCA000000;
            //ROM:01BA6D50 4                       MOV     R3, R3,ROR#2
            R3 = ((R3 >> 2) | (R3 << 30));
            //ROM:01BA6D54 4                       LDR     R5, [SP,#0x50+var_38]
            R5 = get_long((SP + 0x50 - 0x38));
            //ROM:01BA6D58 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6D5C 4                       LDR     R4, [SP,#0x50+var_50]
            R4 = get_long((SP + 0x50 - 0x50));
            //ROM:01BA6D60 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6D64 4                       LDR     R4, [SP,#0x50+var_18]
            R4 = get_long((SP + 0x50 - 0x18));
            //ROM:01BA6D68 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6D6C 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6D70 4                       EOR     R4, R0, R3
            R4 = R0 ^ R3;
            //ROM:01BA6D74 4                       EOR     R4, R4, LR
            R4 = R4 ^ LR;
            //ROM:01BA6D78 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6D7C 4                       ADD     R4, R4, R2,ROR#27
            R4 = R4 + ((R2 >> 27) | (R2 << 5));
            //ROM:01BA6D80 4                       ADD     R12, R4, R12
            R12 = R4 + R12;
            //ROM:01BA6D84 4                       STR     R5, [SP,#0x50+var_18]
            put_long((SP + 0x50 - 0x18), R5);
            //ROM:01BA6D88 4                       LDR     R5, [SP,#0x50+var_34]
            R5 = get_long((SP + 0x50 - 0x34));
            //ROM:01BA6D8C 4                       LDR     R4, [SP,#0x50+var_20]
            R4 = get_long((SP + 0x50 - 0x20));
            //ROM:01BA6D90 4                       ADD     R12, R12, #0xD6
            R12 = R12 + 0xD6;
            //ROM:01BA6D94 4                       EOR     R5, R4, R5
            R5 = R4 ^ R5;
            //ROM:01BA6D98 4                       LDR     R4, [SP,#0x50+var_4C]
            R4 = get_long((SP + 0x50 - 0x4C));
            //ROM:01BA6D9C 4                       MOV     R0, R0,ROR#2
            R0 = ((R0 >> 2) | (R0 << 30));
            //ROM:01BA6DA0 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01BA6DA4 4                       LDR     R4, [SP,#0x50+var_14]
            R4 = get_long((SP + 0x50 - 0x14));
            //ROM:01BA6DA8 4                       ADD     R12, R12, #0xC100
            R12 = R12 + 0xC100;
            //ROM:01BA6DAC 4                       EOR     R4, R5, R4
            R4 = R5 ^ R4;
            //ROM:01BA6DB0 4                       MOV     R5, R4,ROR#31
            R5 = ((R4 >> 31) | (R4 << 1));
            //ROM:01BA6DB4 4                       EOR     R4, R2, R0
            R4 = R2 ^ R0;
            //ROM:01BA6DB8 4                       EOR     R4, R4, R3
            R4 = R4 ^ R3;
            //ROM:01BA6DBC 4                       ADD     R12, R12, #0x620000
            R12 = R12 + 0x620000;
            //ROM:01BA6DC0 4                       ADD     R12, R12, #0xCA000000
            R12 = R12 + 0xCA000000;
            //ROM:01BA6DC4 4                       ADD     R4, R5, R4
            R4 = R5 + R4;
            //ROM:01BA6DC8 4                       ADD     R4, R4, R12,ROR#27
            R4 = R4 + ((R12 >> 27) | (R12 << 5));
            //ROM:01BA6DCC 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA6DD0 4                       ADD     LR, LR, #0xD6
            LR = LR + 0xD6;
            //ROM:01BA6DD4 4                       ADD     LR, LR, #0xC100
            LR = LR + 0xC100;
            //ROM:01BA6DD8 4                       STR     R5, [SP,#0x50+var_14]
            put_long((SP + 0x50 - 0x14), R5);
            //ROM:01BA6DDC 4                       LDR     R4, [R1]
            R4 = get_long(R1);
            //ROM:01BA6DE0 4                       ADD     LR, LR, #0x620000
            LR = LR + 0x620000;
            //ROM:01BA6DE4 4                       ADD     LR, LR, #0xCA000000
            LR = LR + 0xCA000000;
            //ROM:01BA6DE8 4                       ADD     LR, R4, LR
            LR = R4 + LR;
            //ROM:01BA6DEC 4                       STR     LR, [R1]
            put_long(R1, LR);
            //ROM:01BA6DF0 4                       LDR     LR, [R1,#4]
            LR = get_long((R1 + 4));
            //ROM:01BA6DF4 4                       ADD     R12, LR, R12
            R12 = LR + R12;
            //ROM:01BA6DF8 4                       STR     R12, [R1,#4]
            put_long((R1 + 4), R12);
            //ROM:01BA6DFC 4                       LDR     R12, [R1,#8]
            R12 = get_long((R1 + 8));
            //ROM:01BA6E00 4                       ADD     R2, R12, R2,ROR#2
            R2 = R12 + ((R2 >> 2) | (R2 << 30));
            //ROM:01BA6E04 4                       STR     R2, [R1,#8]
            put_long((R1 + 8), R2);
            //ROM:01BA6E08 4                       LDR     R2, [R1,#0xC]
            R2 = get_long((R1 + 0xC));
            //ROM:01BA6E0C 4                       ADD     R0, R2, R0
            R0 = R2 + R0;
            //ROM:01BA6E10 4                       STR     R0, [R1,#0xC]
            put_long((R1 + 0xC), R0);
            //ROM:01BA6E14 4                       LDR     R0, [R1,#0x10]
            R0 = get_long((R1 + 0x10));
            //ROM:01BA6E18 4                       ADD     R0, R0, R3
            R0 = R0 + R3;
            //ROM:01BA6E1C 4                       STR     R0, [R1,#0x10]
            put_long((R1 + 0x10), R0);
            //ROM:01BA6E20 8                       MOV     R1, 0x14A
            R1 = 0x14A;
            //ROM:01BA6E28 4                       ADR     R0, unk_1BA6E38
            R0 = 0x1BA6E38;
            //ROM:01BA6E2C 4                       BL      SistemPlanirov  ; R0 èìÿ çàäàíèÿ
            SistemPlanirov();
            //ROM:01BA6E2C                                                 ; R1 êîìàíäà èëè ðàçìåð
            //ROM:01BA6E30 4                       ADD     SP, SP, #0x40
            SP = SP + 0x40;
            //ROM:01BA6E34 4                       LDMFD   SP!, {R4-R6,PC}
            R4 = get_long(SP + 0x0);
            R5 = get_long(SP + 0x4);
            R6 = get_long(SP + 0x8);
            PC = get_long(SP + 0xC);
            SP = SP + 0x10;

            //

        }

        private void Subcryptography_2()
        {
            //ROM:01DB81BC 4                       STMFD   SP!, {R4-R10,LR}
            put_long(SP - 0x4, LR);
            put_long(SP - 0x8, R10);
            put_long(SP - 0xC, R9);
            put_long(SP - 0x10, R8);
            put_long(SP - 0x14, R7);
            put_long(SP - 0x18, R6);
            put_long(SP - 0x1C, R5);
            put_long(SP - 0x20, R4);
            SP = SP - 0x20;
            //ROM:01DB81C0 4                       LDRB    R5, [R0]
            R5 = get_byte(R0);
            //ROM:01DB81C4 4                       LDRB    R4, [R0,#2]
            R4 = get_byte((R0 + 2));
            //ROM:01DB81C8 4                       LDRB    R6, [R0,#1]
            R6 = get_byte((R0 + 1));
            //ROM:01DB81CC 4                       LDRB    R7, [R0,#3]
            R7 = get_byte((R0 + 3));
            //ROM:01DB81D0 4                       ADD     R0, R5, R4
            R0 = R5 + R4;
            //ROM:01DB81D4 4                       ADD     R2, R0, #0x47
            R2 = R0 + 0x47;
            //ROM:01DB81D8 4                       ADD     R2, R2, #0x8600
            R2 = R2 + 0x8600;
            //ROM:01DB81DC 4                       ADD     R2, R2, #loc_1C80000
            R2 = R2 + 0x1C80000;
            //ROM:01DB81E0 4                       ADD     R2, R2, #0x60000000
            R2 = R2 + 0x60000000;
            //ROM:01DB81E4 4                       LDR     LR, =unk_2247E1C
            LR = 0x2247E1C;
            //ROM:01DB81E8 4                       AND     R0, R2, #0xFF
            R0 = R2 & 0xFF;
            //ROM:01DB81EC 4                       LDR     R8, [LR,R0,LSL#2]
            R8 = get_long((LR + (R0 << 2)));
            //ROM:01DB81F0 4                       MOV     R0, #0xFF
            R0 = 0xFF;
            //ROM:01DB81F4 4                       LDR     R12, =unk_224821C
            R12 = 0x224821C;
            //ROM:01DB81F8 4                       AND     R3, R0, R2,LSR#8
            R3 = R0 & (R2 >> 8);
            //ROM:01DB81FC 4                       LDR     R3, [R12,R3,LSL#2]
            R3 = get_long((R12 + (R3 << 2)));
            //ROM:01DB8200 4                       AND     R9, R0, R2,LSR#16
            R9 = R0 & (R2 >> 16);
            //ROM:01DB8204 4                       EOR     R8, R8, R3
            R8 = R8 ^ R3;
            //ROM:01DB8208 4                       LDR     R3, =unk_224861C
            R3 = 0x224861C;
            //ROM:01DB820C 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8210 4                       EOR     R9, R8, R9
            R9 = R8 ^ R9;
            //ROM:01DB8214 4                       MOV     R8, R2,LSR#24
            R8 = (R2 >> 24);
            //ROM:01DB8218 4                       LDR     R2, =unk_2248A1C
            R2 = 0x2248A1C;
            //ROM:01DB821C 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB8220 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB8224 4                       STR     R8, [R1]
            put_long(R1, R8);
            //ROM:01DB8228 4                       SUB     R8, R6, R7
            R8 = R6 - R7;
            //ROM:01DB822C 4                       ADD     R8, R8, #0xB9
            R8 = R8 + 0xB9;
            //ROM:01DB8230 4                       SUB     R8, R8, #0x8700
            R8 = R8 - 0x8700;
            //ROM:01DB8234 4                       SUB     R8, R8, #loc_1C80000
            R8 = R8 - 0x1C80000;
            //ROM:01DB8238 4                       ADD     R8, R8, #0xA0000000
            R8 = R8 + 0xA0000000;
            //ROM:01DB823C 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB8240 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB8244 4                       AND     R9, R0, R8,LSR#8
            R9 = R0 & (R8 >> 8);
            //ROM:01DB8248 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB824C 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8250 4                       AND     R10, R0, R8,LSR#16
            R10 = R0 & (R8 >> 16);
            //ROM:01DB8254 4                       LDR     R10, [R3,R10,LSL#2]
            R10 = get_long((R3 + (R10 << 2)));
            //ROM:01DB8258 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB825C 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB8260 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8264 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB8268 4                       STR     R8, [R1,#4]
            put_long((R1 + 4), R8);
            //ROM:01DB826C 4                       MOV     R8, R5
            R8 = R5;
            //ROM:01DB8270 4                       MOV     R5, R5,LSR#8
            R5 = (R5 >> 8);
            //ROM:01DB8274 4                       ORR     R5, R5, R6,LSL#24
            R5 = R5 | (R6 << 24);
            //ROM:01DB8278 4                       MOV     R6, R6,LSR#8
            R6 = (R6 >> 8);
            //ROM:01DB827C 4                       ORR     R6, R6, R8,LSL#24
            R6 = R6 | (R8 << 24);
            //ROM:01DB8280 4                       ADD     R8, R5, R4
            R8 = R5 + R4;
            //ROM:01DB8284 4                       ADD     R8, R8, #0x8D
            R8 = R8 + 0x8D;
            //ROM:01DB8288 4                       ADD     R8, R8, #0x10C00
            R8 = R8 + 0x10C00;
            //ROM:01DB828C 4                       ADD     R8, R8, #0x3900000
            R8 = R8 + 0x3900000;
            //ROM:01DB8290 4                       ADD     R8, R8, #0xC0000000
            R8 = R8 + 0xC0000000;
            //ROM:01DB8294 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB8298 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB829C 4                       AND     R9, R0, R8,LSR#8
            R9 = R0 & (R8 >> 8);
            //ROM:01DB82A0 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB82A4 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB82A8 4                       AND     R9, R0, R8,LSR#16
            R9 = R0 & (R8 >> 16);
            //ROM:01DB82AC 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB82B0 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB82B4 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB82B8 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB82BC 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB82C0 4                       STR     R8, [R1,#8]
            put_long((R1 + 8), R8);
            //ROM:01DB82C4 4                       SUB     R8, R6, R7
            R8 = R6 - R7;
            //ROM:01DB82C8 4                       SUB     R8, R8, #0x8D
            R8 = R8 - 0x8D;
            //ROM:01DB82CC 4                       SUB     R8, R8, #0x10C00
            R8 = R8 - 0x10C00;
            //ROM:01DB82D0 4                       SUB     R8, R8, #0x3900000
            R8 = R8 - 0x3900000;
            //ROM:01DB82D4 4                       ADD     R8, R8, #0x40000000
            R8 = R8 + 0x40000000;
            //ROM:01DB82D8 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB82DC 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB82E0 4                       AND     R9, R0, R8,LSR#8
            R9 = R0 & (R8 >> 8);
            //ROM:01DB82E4 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB82E8 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB82EC 4                       AND     R10, R0, R8,LSR#16
            R10 = R0 & (R8 >> 16);
            //ROM:01DB82F0 4                       LDR     R10, [R3,R10,LSL#2]
            R10 = get_long((R3 + (R10 << 2)));
            //ROM:01DB82F4 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB82F8 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB82FC 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8300 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB8304 4                       STR     R8, [R1,#0xC]
            put_long((R1 + 0xC), R8);
            //ROM:01DB8308 4                       MOV     R8, R4
            R8 = R4;
            //ROM:01DB830C 4                       MOV     R4, R4,LSL#8
            R4 = (R4 << 8);
            //ROM:01DB8310 4                       ORR     R4, R4, R7,LSR#24
            R4 = R4 | (R7 >> 24);
            //ROM:01DB8314 4                       MOV     R7, R7,LSL#8
            R7 = (R7 << 8);
            //ROM:01DB8318 4                       ORR     R7, R7, R8,LSR#24
            R7 = R7 | (R8 >> 24);
            //ROM:01DB831C 4                       ADD     R8, R5, R4
            R8 = R5 + R4;
            //ROM:01DB8320 4                       ADD     R8, R8, #0x8000001A
            R8 = R8 + 0x8000001A;
            //ROM:01DB8324 4                       ADD     R8, R8, #0x1900
            R8 = R8 + 0x1900;
            //ROM:01DB8328 4                       SUB     R8, R8, #0xDE0000
            R8 = R8 - 0xDE0000;
            //ROM:01DB832C 4                       ADD     R8, R8, #0x8000000
            R8 = R8 + 0x8000000;
            //ROM:01DB8330 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB8334 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB8338 4                       AND     R9, R0, R8,LSR#8
            R9 = R0 & (R8 >> 8);
            //ROM:01DB833C 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8340 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8344 4                       AND     R9, R0, R8,LSR#16
            R9 = R0 & (R8 >> 16);
            //ROM:01DB8348 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB834C 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB8350 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB8354 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8358 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB835C 4                       STR     R8, [R1,#0x10]
            put_long((R1 + 0x10), R8);
            //ROM:01DB8360 4                       SUB     R8, R6, R7
            R8 = R6 - R7;
            //ROM:01DB8364 4                       ADD     R8, R8, #0xE6
            R8 = R8 + 0xE6;
            //ROM:01DB8368 4                       ADD     R8, R8, #0xE600
            R8 = R8 + 0xE600;
            //ROM:01DB836C 4                       ADD     R8, R8, #0xDD0000
            R8 = R8 + 0xDD0000;
            //ROM:01DB8370 4                       ADD     R8, R8, #0x78000000
            R8 = R8 + 0x78000000;
            //ROM:01DB8374 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB8378 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB837C 4                       AND     R9, R0, R8,LSR#8
            R9 = R0 & (R8 >> 8);
            //ROM:01DB8380 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8384 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8388 4                       AND     R9, R0, R8,LSR#16
            R9 = R0 & (R8 >> 16);
            //ROM:01DB838C 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8390 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB8394 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB8398 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB839C 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB83A0 4                       STR     R8, [R1,#0x14]
            put_long((R1 + 0x14), R8);
            //ROM:01DB83A4 4                       MOV     R8, R5,LSR#8
            R8 = (R5 >> 8);
            //ROM:01DB83A8 4                       ORR     R8, R8, R6,LSL#24
            R8 = R8 | (R6 << 24);
            //ROM:01DB83AC 4                       MOV     R6, R6,LSR#8
            R6 = (R6 >> 8);
            //ROM:01DB83B0 4                       ORR     R5, R6, R5,LSL#24
            R5 = R6 | (R5 << 24);
            //ROM:01DB83B4 4                       ADD     R6, R8, R4
            R6 = R8 + R4;
            //ROM:01DB83B8 4                       ADD     R6, R6, #0x234
            R6 = R6 + 0x234;
            //ROM:01DB83BC 4                       ADD     R6, R6, #0x43000
            R6 = R6 + 0x43000;
            //ROM:01DB83C0 4                       ADD     R6, R6, #0xE400000
            R6 = R6 + 0xE400000;
            //ROM:01DB83C4 4                       AND     R9, R6, #0xFF
            R9 = R6 & 0xFF;
            //ROM:01DB83C8 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB83CC 4                       AND     R9, R0, R6,LSR#8
            R9 = R0 & (R6 >> 8);
            //ROM:01DB83D0 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB83D4 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB83D8 4                       AND     R9, R0, R6,LSR#16
            R9 = R0 & (R6 >> 16);
            //ROM:01DB83DC 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB83E0 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB83E4 4                       LDR     R6, [R2,R6,LSL#2]
            R6 = get_long((R2 + (R6 << 2)));
            //ROM:01DB83E8 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB83EC 4                       EOR     R6, R9, R6
            R6 = R9 ^ R6;
            //ROM:01DB83F0 4                       STR     R6, [R1,#0x18]
            put_long((R1 + 0x18), R6);
            //ROM:01DB83F4 4                       SUB     R6, R5, R7
            R6 = R5 - R7;
            //ROM:01DB83F8 4                       SUB     R6, R6, #0x234
            R6 = R6 - 0x234;
            //ROM:01DB83FC 4                       SUB     R6, R6, #0x43000
            R6 = R6 - 0x43000;
            //ROM:01DB8400 4                       SUB     R6, R6, #0xE400000
            R6 = R6 - 0xE400000;
            //ROM:01DB8404 4                       AND     R9, R6, #0xFF
            R9 = R6 & 0xFF;
            //ROM:01DB8408 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB840C 4                       AND     R9, R0, R6,LSR#8
            R9 = R0 & (R6 >> 8);
            //ROM:01DB8410 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8414 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8418 4                       AND     R9, R0, R6,LSR#16
            R9 = R0 & (R6 >> 16);
            //ROM:01DB841C 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8420 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB8424 4                       LDR     R6, [R2,R6,LSL#2]
            R6 = get_long((R2 + (R6 << 2)));
            //ROM:01DB8428 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB842C 4                       EOR     R6, R9, R6
            R6 = R9 ^ R6;
            //ROM:01DB8430 4                       STR     R6, [R1,#0x1C]
            put_long((R1 + 0x1C), R6);
            //ROM:01DB8434 4                       MOV     R6, R4,LSL#8
            R6 = (R4 << 8);
            //ROM:01DB8438 4                       ORR     R6, R6, R7,LSR#24
            R6 = R6 | (R7 >> 24);
            //ROM:01DB843C 4                       MOV     R7, R7,LSL#8
            R7 = (R7 << 8);
            //ROM:01DB8440 4                       ORR     R4, R7, R4,LSR#24
            R4 = R7 | (R4 >> 24);
            //ROM:01DB8444 4                       ADD     R7, R8, R6
            R7 = R8 + R6;
            //ROM:01DB8448 4                       ADD     R7, R7, #0x67
            R7 = R7 + 0x67;
            //ROM:01DB844C 4                       ADD     R7, R7, #0x6400
            R7 = R7 + 0x6400;
            //ROM:01DB8450 4                       SUB     R7, R7, #0x3780000
            R7 = R7 - 0x3780000;
            //ROM:01DB8454 4                       ADD     R7, R7, #0x20000000
            R7 = R7 + 0x20000000;
            //ROM:01DB8458 4                       AND     R9, R7, #0xFF
            R9 = R7 & 0xFF;
            //ROM:01DB845C 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB8460 4                       AND     R9, R0, R7,LSR#8
            R9 = R0 & (R7 >> 8);
            //ROM:01DB8464 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8468 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB846C 4                       AND     R9, R0, R7,LSR#16
            R9 = R0 & (R7 >> 16);
            //ROM:01DB8470 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8474 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB8478 4                       LDR     R7, [R2,R7,LSL#2]
            R7 = get_long((R2 + (R7 << 2)));
            //ROM:01DB847C 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8480 4                       EOR     R7, R9, R7
            R7 = R9 ^ R7;
            //ROM:01DB8484 4                       STR     R7, [R1,#0x20]
            put_long((R1 + 0x20), R7);
            //ROM:01DB8488 4                       SUB     R7, R5, R4
            R7 = R5 - R4;
            //ROM:01DB848C 4                       SUB     R7, R7, #0x67
            R7 = R7 - 0x67;
            //ROM:01DB8490 4                       ADD     R7, R7, #0x39C00
            R7 = R7 + 0x39C00;
            //ROM:01DB8494 4                       ADD     R7, R7, #0x3740000
            R7 = R7 + 0x3740000;
            //ROM:01DB8498 4                       ADD     R7, R7, #0xE0000000
            R7 = R7 + 0xE0000000;
            //ROM:01DB849C 4                       AND     R9, R7, #0xFF
            R9 = R7 & 0xFF;
            //ROM:01DB84A0 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB84A4 4                       AND     R9, R0, R7,LSR#8
            R9 = R0 & (R7 >> 8);
            //ROM:01DB84A8 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB84AC 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB84B0 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB84B4 4                       LDR     R10, [R3,R10,LSL#2]
            R10 = get_long((R3 + (R10 << 2)));
            //ROM:01DB84B8 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB84BC 4                       LDR     R7, [R2,R7,LSL#2]
            R7 = get_long((R2 + (R7 << 2)));
            //ROM:01DB84C0 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB84C4 4                       EOR     R7, R9, R7
            R7 = R9 ^ R7;
            //ROM:01DB84C8 4                       STR     R7, [R1,#0x24]
            put_long((R1 + 0x24), R7);
            //ROM:01DB84CC 4                       MOV     R7, R8,LSR#8
            R7 = (R8 >> 8);
            //ROM:01DB84D0 4                       ORR     R7, R7, R5,LSL#24
            R7 = R7 | (R5 << 24);
            //ROM:01DB84D4 4                       MOV     R5, R5,LSR#8
            R5 = (R5 >> 8);
            //ROM:01DB84D8 4                       ORR     R5, R5, R8,LSL#24
            R5 = R5 | (R8 << 24);
            //ROM:01DB84DC 4                       ADD     R8, R7, R6
            R8 = R7 + R6;
            //ROM:01DB84E0 4                       ADD     R8, R8, #0xCD
            R8 = R8 + 0xCD;
            //ROM:01DB84E4 4                       ADD     R8, R8, #0xC800
            R8 = R8 + 0xC800;
            //ROM:01DB84E8 4                       SUB     R8, R8, #0x6F00000
            R8 = R8 - 0x6F00000;
            //ROM:01DB84EC 4                       ADD     R8, R8, #0x40000000
            R8 = R8 + 0x40000000;
            //ROM:01DB84F0 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB84F4 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB84F8 4                       AND     R9, R0, R8,LSR#8
            R9 = R0 & (R8 >> 8);
            //ROM:01DB84FC 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8500 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8504 4                       AND     R9, R0, R8,LSR#16
            R9 = R0 & (R8 >> 16);
            //ROM:01DB8508 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB850C 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB8510 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB8514 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8518 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB851C 4                       STR     R8, [R1,#0x28]
            put_long((R1 + 0x28), R8);
            //ROM:01DB8520 4                       SUB     R8, R5, R4
            R8 = R5 - R4;
            //ROM:01DB8524 4                       SUB     R8, R8, #0xCD
            R8 = R8 - 0xCD;
            //ROM:01DB8528 4                       SUB     R8, R8, #0xC800
            R8 = R8 - 0xC800;
            //ROM:01DB852C 4                       ADD     R8, R8, #0x6F00000
            R8 = R8 + 0x6F00000;
            //ROM:01DB8530 4                       ADD     R8, R8, #0xC0000000
            R8 = R8 + 0xC0000000;
            //ROM:01DB8534 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB8538 4                       AND     R10, R0, R8,LSR#8
            R10 = R0 & (R8 >> 8);
            //ROM:01DB853C 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8540 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8544 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8548 4                       AND     R10, R0, R8,LSR#16
            R10 = R0 & (R8 >> 16);
            //ROM:01DB854C 4                       LDR     R10, [R3,R10,LSL#2]
            R10 = get_long((R3 + (R10 << 2)));
            //ROM:01DB8550 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB8554 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB8558 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB855C 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB8560 4                       STR     R8, [R1,#0x2C]
            put_long((R1 + 0x2C), R8);
            //ROM:01DB8564 4                       MOV     R8, R6,LSL#8
            R8 = (R6 << 8);
            //ROM:01DB8568 4                       ORR     R8, R8, R4,LSR#24
            R8 = R8 | (R4 >> 24);
            //ROM:01DB856C 4                       MOV     R4, R4,LSL#8
            R4 = (R4 << 8);
            //ROM:01DB8570 4                       ORR     R4, R4, R6,LSR#24
            R4 = R4 | (R6 >> 24);
            //ROM:01DB8574 4                       ADD     R6, R7, R8
            R6 = R7 + R8;
            //ROM:01DB8578 4                       ADD     R6, R6, #0x99
            R6 = R6 + 0x99;
            //ROM:01DB857C 4                       ADD     R6, R6, #0x9100
            R6 = R6 + 0x9100;
            //ROM:01DB8580 4                       ADD     R6, R6, #0x210000
            R6 = R6 + 0x210000;
            //ROM:01DB8584 4                       ADD     R6, R6, #0x72000000
            R6 = R6 + 0x72000000;
            //ROM:01DB8588 4                       AND     R9, R6, #0xFF
            R9 = R6 & 0xFF;
            //ROM:01DB858C 4                       AND     R10, R0, R6,LSR#8
            R10 = R0 & (R6 >> 8);
            //ROM:01DB8590 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8594 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8598 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB859C 4                       AND     R10, R0, R6,LSR#16
            R10 = R0 & (R6 >> 16);
            //ROM:01DB85A0 4                       LDR     R10, [R3,R10,LSL#2]
            R10 = get_long((R3 + (R10 << 2)));
            //ROM:01DB85A4 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB85A8 4                       LDR     R6, [R2,R6,LSL#2]
            R6 = get_long((R2 + (R6 << 2)));
            //ROM:01DB85AC 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB85B0 4                       EOR     R6, R9, R6
            R6 = R9 ^ R6;
            //ROM:01DB85B4 4                       STR     R6, [R1,#0x30]
            put_long((R1 + 0x30), R6);
            //ROM:01DB85B8 4                       SUB     R6, R5, R4
            R6 = R5 - R4;
            //ROM:01DB85BC 4                       ADD     R6, R6, #0x67
            R6 = R6 + 0x67;
            //ROM:01DB85C0 4                       ADD     R6, R6, #0x6E00
            R6 = R6 + 0x6E00;
            //ROM:01DB85C4 4                       ADD     R6, R6, #0xDE0000
            R6 = R6 + 0xDE0000;
            //ROM:01DB85C8 4                       ADD     R6, R6, #0x8D000000
            R6 = R6 + 0x8D000000;
            //ROM:01DB85CC 4                       AND     R9, R6, #0xFF
            R9 = R6 & 0xFF;
            //ROM:01DB85D0 4                       AND     R10, R0, R6,LSR#8
            R10 = R0 & (R6 >> 8);
            //ROM:01DB85D4 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB85D8 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB85DC 4                       EOR     R10, R9, R10
            R10 = R9 ^ R10;
            //ROM:01DB85E0 4                       AND     R9, R0, R6,LSR#16
            R9 = R0 & (R6 >> 16);
            //ROM:01DB85E4 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB85E8 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB85EC 4                       LDR     R6, [R2,R6,LSL#2]
            R6 = get_long((R2 + (R6 << 2)));
            //ROM:01DB85F0 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB85F4 4                       EOR     R6, R9, R6
            R6 = R9 ^ R6;
            //ROM:01DB85F8 4                       STR     R6, [R1,#0x34]
            put_long((R1 + 0x34), R6);
            //ROM:01DB85FC 4                       MOV     R6, R7,LSR#8
            R6 = (R7 >> 8);
            //ROM:01DB8600 4                       ORR     R6, R6, R5,LSL#24
            R6 = R6 | (R5 << 24);
            //ROM:01DB8604 4                       MOV     R5, R5,LSR#8
            R5 = (R5 >> 8);
            //ROM:01DB8608 4                       ORR     R5, R5, R7,LSL#24
            R5 = R5 | (R7 << 24);
            //ROM:01DB860C 4                       ADD     R7, R6, R8
            R7 = R6 + R8;
            //ROM:01DB8610 4                       SUB     R7, R7, #0xCF
            R7 = R7 - 0xCF;
            //ROM:01DB8614 4                       ADD     R7, R7, #0x32400
            R7 = R7 + 0x32400;
            //ROM:01DB8618 4                       SUB     R7, R7, #0x1BC00000
            R7 = R7 - 0x1BC00000;
            //ROM:01DB861C 4                       AND     R9, R7, #0xFF
            R9 = R7 & 0xFF;
            //ROM:01DB8620 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB8624 4                       AND     R9, R0, R7,LSR#8
            R9 = R0 & (R7 >> 8);
            //ROM:01DB8628 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB862C 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8630 4                       AND     R9, R0, R7,LSR#16
            R9 = R0 & (R7 >> 16);
            //ROM:01DB8634 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8638 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB863C 4                       LDR     R7, [R2,R7,LSL#2]
            R7 = get_long((R2 + (R7 << 2)));
            //ROM:01DB8640 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8644 4                       EOR     R7, R9, R7
            R7 = R9 ^ R7;
            //ROM:01DB8648 4                       STR     R7, [R1,#0x38]
            put_long((R1 + 0x38), R7);
            //ROM:01DB864C 4                       SUB     R7, R5, R4
            R7 = R5 - R4;
            //ROM:01DB8650 4                       ADD     R7, R7, #0xCF
            R7 = R7 + 0xCF;
            //ROM:01DB8654 4                       SUB     R7, R7, #0x32400
            R7 = R7 - 0x32400;
            //ROM:01DB8658 4                       ADD     R7, R7, #0x1BC00000
            R7 = R7 + 0x1BC00000;
            //ROM:01DB865C 4                       AND     R9, R7, #0xFF
            R9 = R7 & 0xFF;
            //ROM:01DB8660 4                       AND     R10, R0, R7,LSR#8
            R10 = R0 & (R7 >> 8);
            //ROM:01DB8664 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8668 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB866C 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8670 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB8674 4                       LDR     R10, [R3,R10,LSL#2]
            R10 = get_long((R3 + (R10 << 2)));
            //ROM:01DB8678 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB867C 4                       LDR     R7, [R2,R7,LSL#2]
            R7 = get_long((R2 + (R7 << 2)));
            //ROM:01DB8680 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8684 4                       EOR     R7, R9, R7
            R7 = R9 ^ R7;
            //ROM:01DB8688 4                       STR     R7, [R1,#0x3C]
            put_long((R1 + 0x3C), R7);
            //ROM:01DB868C 4                       MOV     R7, R8,LSL#8
            R7 = (R8 << 8);
            //ROM:01DB8690 4                       ORR     R7, R7, R4,LSR#24
            R7 = R7 | (R4 >> 24);
            //ROM:01DB8694 4                       MOV     R4, R4,LSL#8
            R4 = (R4 << 8);
            //ROM:01DB8698 4                       ORR     R4, R4, R8,LSR#24
            R4 = R4 | (R8 >> 24);
            //ROM:01DB869C 4                       ADD     R8, R6, R7
            R8 = R6 + R7;
            //ROM:01DB86A0 4                       ADD     R8, R8, #0x62
            R8 = R8 + 0x62;
            //ROM:01DB86A4 4                       ADD     R8, R8, #0x4600
            R8 = R8 + 0x4600;
            //ROM:01DB86A8 4                       ADD     R8, R8, #0x860000
            R8 = R8 + 0x860000;
            //ROM:01DB86AC 4                       ADD     R8, R8, #0xC8000000
            R8 = R8 + 0xC8000000;
            //ROM:01DB86B0 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB86B4 4                       AND     R10, R0, R8,LSR#8
            R10 = R0 & (R8 >> 8);
            //ROM:01DB86B8 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB86BC 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB86C0 4                       EOR     R10, R9, R10
            R10 = R9 ^ R10;
            //ROM:01DB86C4 4                       AND     R9, R0, R8,LSR#16
            R9 = R0 & (R8 >> 16);
            //ROM:01DB86C8 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB86CC 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB86D0 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB86D4 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB86D8 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB86DC 4                       STR     R8, [R1,#0x40]
            put_long((R1 + 0x40), R8);
            //ROM:01DB86E0 4                       SUB     R8, R5, R4
            R8 = R5 - R4;
            //ROM:01DB86E4 4                       ADD     R8, R8, #0x9E
            R8 = R8 + 0x9E;
            //ROM:01DB86E8 4                       ADD     R8, R8, #0xB900
            R8 = R8 + 0xB900;
            //ROM:01DB86EC 4                       SUB     R8, R8, #0x870000
            R8 = R8 - 0x870000;
            //ROM:01DB86F0 4                       ADD     R8, R8, #0x38000000
            R8 = R8 + 0x38000000;
            //ROM:01DB86F4 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB86F8 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB86FC 4                       AND     R9, R0, R8,LSR#8
            R9 = R0 & (R8 >> 8);
            //ROM:01DB8700 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8704 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8708 4                       AND     R9, R0, R8,LSR#16
            R9 = R0 & (R8 >> 16);
            //ROM:01DB870C 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8710 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB8714 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB8718 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB871C 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB8720 4                       STR     R8, [R1,#0x44]
            put_long((R1 + 0x44), R8);
            //ROM:01DB8724 4                       MOV     R8, R6,LSR#8
            R8 = (R6 >> 8);
            //ROM:01DB8728 4                       ORR     R8, R8, R5,LSL#24
            R8 = R8 | (R5 << 24);
            //ROM:01DB872C 4                       MOV     R5, R5,LSR#8
            R5 = (R5 >> 8);
            //ROM:01DB8730 4                       ORR     R5, R5, R6,LSL#24
            R5 = R5 | (R6 << 24);
            //ROM:01DB8734 4                       ADD     R6, R8, R7
            R6 = R8 + R7;
            //ROM:01DB8738 4                       SUB     R6, R6, #0x33C
            R6 = R6 - 0x33C;
            //ROM:01DB873C 4                       ADD     R6, R6, #0xC9000
            R6 = R6 + 0xC9000;
            //ROM:01DB8740 4                       ADD     R6, R6, #0x91000000
            R6 = R6 + 0x91000000;
            //ROM:01DB8744 4                       AND     R9, R6, #0xFF
            R9 = R6 & 0xFF;
            //ROM:01DB8748 4                       AND     R10, R0, R6,LSR#8
            R10 = R0 & (R6 >> 8);
            //ROM:01DB874C 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8750 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8754 4                       EOR     R10, R9, R10
            R10 = R9 ^ R10;
            //ROM:01DB8758 4                       AND     R9, R0, R6,LSR#16
            R9 = R0 & (R6 >> 16);
            //ROM:01DB875C 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8760 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB8764 4                       LDR     R6, [R2,R6,LSL#2]
            R6 = get_long((R2 + (R6 << 2)));
            //ROM:01DB8768 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB876C 4                       EOR     R6, R9, R6
            R6 = R9 ^ R6;
            //ROM:01DB8770 4                       STR     R6, [R1,#0x48]
            put_long((R1 + 0x48), R6);
            //ROM:01DB8774 4                       SUB     R6, R5, R4
            R6 = R5 - R4;
            //ROM:01DB8778 4                       ADD     R6, R6, #0x33C
            R6 = R6 + 0x33C;
            //ROM:01DB877C 4                       SUB     R6, R6, #0xC9000
            R6 = R6 - 0xC9000;
            //ROM:01DB8780 4                       ADD     R6, R6, #0x6F000000
            R6 = R6 + 0x6F000000;
            //ROM:01DB8784 4                       AND     R9, R6, #0xFF
            R9 = R6 & 0xFF;
            //ROM:01DB8788 4                       AND     R10, R0, R6,LSR#8
            R10 = R0 & (R6 >> 8);
            //ROM:01DB878C 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8790 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8794 4                       EOR     R10, R9, R10
            R10 = R9 ^ R10;
            //ROM:01DB8798 4                       AND     R9, R0, R6,LSR#16
            R9 = R0 & (R6 >> 16);
            //ROM:01DB879C 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB87A0 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB87A4 4                       LDR     R6, [R2,R6,LSL#2]
            R6 = get_long((R2 + (R6 << 2)));
            //ROM:01DB87A8 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB87AC 4                       EOR     R6, R9, R6
            R6 = R9 ^ R6;
            //ROM:01DB87B0 4                       STR     R6, [R1,#0x4C]
            put_long((R1 + 0x4C), R6);
            //ROM:01DB87B4 4                       MOV     R6, R7,LSL#8
            R6 = (R7 << 8);
            //ROM:01DB87B8 4                       ORR     R6, R6, R4,LSR#24
            R6 = R6 | (R4 >> 24);
            //ROM:01DB87BC 4                       MOV     R4, R4,LSL#8
            R4 = (R4 << 8);
            //ROM:01DB87C0 4                       ORR     R4, R4, R7,LSR#24
            R4 = R4 | (R7 >> 24);
            //ROM:01DB87C4 4                       ADD     R7, R8, R6
            R7 = R8 + R6;
            //ROM:01DB87C8 4                       ADD     R7, R7, #0x188
            R7 = R7 + 0x188;
            //ROM:01DB87CC 4                       ADD     R7, R7, #0x11800
            R7 = R7 + 0x11800;
            //ROM:01DB87D0 4                       ADD     R7, R7, #unk_2180000
            R7 = R7 + 0x2180000;
            //ROM:01DB87D4 4                       ADD     R7, R7, #0x20000000
            R7 = R7 + 0x20000000;
            //ROM:01DB87D8 4                       AND     R9, R7, #0xFF
            R9 = R7 & 0xFF;
            //ROM:01DB87DC 4                       AND     R10, R0, R7,LSR#8
            R10 = R0 & (R7 >> 8);
            //ROM:01DB87E0 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB87E4 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB87E8 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB87EC 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB87F0 4                       LDR     R10, [R3,R10,LSL#2]
            R10 = get_long((R3 + (R10 << 2)));
            //ROM:01DB87F4 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB87F8 4                       LDR     R7, [R2,R7,LSL#2]
            R7 = get_long((R2 + (R7 << 2)));
            //ROM:01DB87FC 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8800 4                       EOR     R7, R9, R7
            R7 = R9 ^ R7;
            //ROM:01DB8804 4                       STR     R7, [R1,#0x50]
            put_long((R1 + 0x50), R7);
            //ROM:01DB8808 4                       SUB     R7, R5, R4
            R7 = R5 - R4;
            //ROM:01DB880C 4                       ADD     R7, R7, #0x278
            R7 = R7 + 0x278;
            //ROM:01DB8810 4                       ADD     R7, R7, #0x2E400
            R7 = R7 + 0x2E400;
            //ROM:01DB8814 4                       SUB     R7, R7, #unk_21C0000
            R7 = R7 - 0x21C0000;
            //ROM:01DB8818 4                       ADD     R7, R7, #0xE0000000
            R7 = R7 + 0xE0000000;
            //ROM:01DB881C 4                       AND     R9, R7, #0xFF
            R9 = R7 & 0xFF;
            //ROM:01DB8820 4                       AND     R10, R0, R7,LSR#8
            R10 = R0 & (R7 >> 8);
            //ROM:01DB8824 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8828 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB882C 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8830 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB8834 4                       LDR     R10, [R3,R10,LSL#2]
            R10 = get_long((R3 + (R10 << 2)));
            //ROM:01DB8838 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB883C 4                       LDR     R7, [R2,R7,LSL#2]
            R7 = get_long((R2 + (R7 << 2)));
            //ROM:01DB8840 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8844 4                       EOR     R7, R9, R7
            R7 = R9 ^ R7;
            //ROM:01DB8848 4                       STR     R7, [R1,#0x54]
            put_long((R1 + 0x54), R7);
            //ROM:01DB884C 4                       MOV     R7, R8,LSR#8
            R7 = (R8 >> 8);
            //ROM:01DB8850 4                       ORR     R7, R7, R5,LSL#24
            R7 = R7 | (R5 << 24);
            //ROM:01DB8854 4                       MOV     R5, R5,LSR#8
            R5 = (R5 >> 8);
            //ROM:01DB8858 4                       ORR     R5, R5, R8,LSL#24
            R5 = R5 | (R8 << 24);
            //ROM:01DB885C 4                       ADD     R8, R7, R6
            R8 = R7 + R6;
            //ROM:01DB8860 4                       SUB     R8, R8, #0xF1
            R8 = R8 - 0xF1;
            //ROM:01DB8864 4                       ADD     R8, R8, #0x23400
            R8 = R8 + 0x23400;
            //ROM:01DB8868 4                       ADD     R8, R8, #0x4300000
            R8 = R8 + 0x4300000;
            //ROM:01DB886C 4                       ADD     R8, R8, #0x40000000
            R8 = R8 + 0x40000000;
            //ROM:01DB8870 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB8874 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB8878 4                       AND     R9, R0, R8,LSR#8
            R9 = R0 & (R8 >> 8);
            //ROM:01DB887C 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8880 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8884 4                       AND     R9, R0, R8,LSR#16
            R9 = R0 & (R8 >> 16);
            //ROM:01DB8888 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB888C 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB8890 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB8894 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8898 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB889C 4                       STR     R8, [R1,#0x58]
            put_long((R1 + 0x58), R8);
            //ROM:01DB88A0 4                       SUB     R8, R5, R4
            R8 = R5 - R4;
            //ROM:01DB88A4 4                       ADD     R8, R8, #0xF1
            R8 = R8 + 0xF1;
            //ROM:01DB88A8 4                       SUB     R8, R8, #0x23400
            R8 = R8 - 0x23400;
            //ROM:01DB88AC 4                       SUB     R8, R8, #0x4300000
            R8 = R8 - 0x4300000;
            //ROM:01DB88B0 4                       ADD     R8, R8, #0xC0000000
            R8 = R8 + 0xC0000000;
            //ROM:01DB88B4 4                       AND     R9, R8, #0xFF
            R9 = R8 & 0xFF;
            //ROM:01DB88B8 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB88BC 4                       AND     R9, R0, R8,LSR#8
            R9 = R0 & (R8 >> 8);
            //ROM:01DB88C0 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB88C4 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB88C8 4                       AND     R10, R0, R8,LSR#16
            R10 = R0 & (R8 >> 16);
            //ROM:01DB88CC 4                       LDR     R10, [R3,R10,LSL#2]
            R10 = get_long((R3 + (R10 << 2)));
            //ROM:01DB88D0 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB88D4 4                       LDR     R8, [R2,R8,LSL#2]
            R8 = get_long((R2 + (R8 << 2)));
            //ROM:01DB88D8 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB88DC 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB88E0 4                       STR     R8, [R1,#0x5C]
            put_long((R1 + 0x5C), R8);
            //ROM:01DB88E4 4                       MOV     R8, R6,LSL#8
            R8 = (R6 << 8);
            //ROM:01DB88E8 4                       ORR     R8, R8, R4,LSR#24
            R8 = R8 | (R4 >> 24);
            //ROM:01DB88EC 4                       MOV     R4, R4,LSL#8
            R4 = (R4 << 8);
            //ROM:01DB88F0 4                       ORR     R4, R4, R6,LSR#24
            R4 = R4 | (R6 >> 24);
            //ROM:01DB88F4 4                       ADD     R6, R7, R8
            R6 = R7 + R8;
            //ROM:01DB88F8 4                       ADD     R6, R6, #0x8000001D
            R6 = R6 + 0x8000001D;
            //ROM:01DB88FC 4                       ADD     R6, R6, #0x6600
            R6 = R6 + 0x6600;
            //ROM:01DB8900 4                       ADD     R6, R6, #0x640000
            R6 = R6 + 0x640000;
            //ROM:01DB8904 4                       ADD     R6, R6, #0x8000000
            R6 = R6 + 0x8000000;
            //ROM:01DB8908 4                       AND     R9, R6, #0xFF
            R9 = R6 & 0xFF;
            //ROM:01DB890C 4                       AND     R10, R0, R6,LSR#8
            R10 = R0 & (R6 >> 8);
            //ROM:01DB8910 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8914 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8918 4                       EOR     R10, R9, R10
            R10 = R9 ^ R10;
            //ROM:01DB891C 4                       AND     R9, R0, R6,LSR#16
            R9 = R0 & (R6 >> 16);
            //ROM:01DB8920 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8924 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB8928 4                       LDR     R6, [R2,R6,LSL#2]
            R6 = get_long((R2 + (R6 << 2)));
            //ROM:01DB892C 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8930 4                       EOR     R6, R9, R6
            R6 = R9 ^ R6;
            //ROM:01DB8934 4                       STR     R6, [R1,#0x60]
            put_long((R1 + 0x60), R6);
            //ROM:01DB8938 4                       SUB     R6, R5, R4
            R6 = R5 - R4;
            //ROM:01DB893C 4                       ADD     R6, R6, #0xE3
            R6 = R6 + 0xE3;
            //ROM:01DB8940 4                       SUB     R6, R6, #0x6700
            R6 = R6 - 0x6700;
            //ROM:01DB8944 4                       ADD     R6, R6, #0x39C0000
            R6 = R6 + 0x39C0000;
            //ROM:01DB8948 4                       ADD     R6, R6, #0x74000000
            R6 = R6 + 0x74000000;
            //ROM:01DB894C 4                       AND     R9, R6, #0xFF
            R9 = R6 & 0xFF;
            //ROM:01DB8950 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB8954 4                       AND     R9, R0, R6,LSR#8
            R9 = R0 & (R6 >> 8);
            //ROM:01DB8958 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB895C 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8960 4                       AND     R9, R0, R6,LSR#16
            R9 = R0 & (R6 >> 16);
            //ROM:01DB8964 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8968 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB896C 4                       LDR     R6, [R2,R6,LSL#2]
            R6 = get_long((R2 + (R6 << 2)));
            //ROM:01DB8970 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8974 4                       EOR     R6, R9, R6
            R6 = R9 ^ R6;
            //ROM:01DB8978 4                       STR     R6, [R1,#0x64]
            put_long((R1 + 0x64), R6);
            //ROM:01DB897C 4                       MOV     R6, R7,LSR#8
            R6 = (R7 >> 8);
            //ROM:01DB8980 4                       ORR     R6, R6, R5,LSL#24
            R6 = R6 | (R5 << 24);
            //ROM:01DB8984 4                       MOV     R5, R5,LSR#8
            R5 = (R5 >> 8);
            //ROM:01DB8988 4                       ORR     R5, R5, R7,LSL#24
            R5 = R5 | (R7 << 24);
            //ROM:01DB898C 4                       ADD     R7, R6, R8
            R7 = R6 + R8;
            //ROM:01DB8990 4                       ADD     R7, R7, #0x3A
            R7 = R7 + 0x3A;
            //ROM:01DB8994 4                       ADD     R7, R7, #0xCC00
            R7 = R7 + 0xCC00;
            //ROM:01DB8998 4                       ADD     R7, R7, #0xC80000
            R7 = R7 + 0xC80000;
            //ROM:01DB899C 4                       ADD     R7, R7, #0x10000000
            R7 = R7 + 0x10000000;
            //ROM:01DB89A0 4                       AND     R9, R7, #0xFF
            R9 = R7 & 0xFF;
            //ROM:01DB89A4 4                       AND     R10, R0, R7,LSR#8
            R10 = R0 & (R7 >> 8);
            //ROM:01DB89A8 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB89AC 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB89B0 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB89B4 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB89B8 4                       LDR     R10, [R3,R10,LSL#2]
            R10 = get_long((R3 + (R10 << 2)));
            //ROM:01DB89BC 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB89C0 4                       LDR     R7, [R2,R7,LSL#2]
            R7 = get_long((R2 + (R7 << 2)));
            //ROM:01DB89C4 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB89C8 4                       EOR     R7, R9, R7
            R7 = R9 ^ R7;
            //ROM:01DB89CC 4                       STR     R7, [R1,#0x68]
            put_long((R1 + 0x68), R7);
            //ROM:01DB89D0 4                       SUB     R7, R5, R4
            R7 = R5 - R4;
            //ROM:01DB89D4 4                       SUB     R7, R7, #0x3A
            R7 = R7 - 0x3A;
            //ROM:01DB89D8 4                       ADD     R7, R7, #0x33400
            R7 = R7 + 0x33400;
            //ROM:01DB89DC 4                       SUB     R7, R7, #0xCC0000
            R7 = R7 - 0xCC0000;
            //ROM:01DB89E0 4                       ADD     R7, R7, #0xF0000000
            R7 = R7 + 0xF0000000;
            //ROM:01DB89E4 4                       AND     R9, R7, #0xFF
            R9 = R7 & 0xFF;
            //ROM:01DB89E8 4                       AND     R10, R0, R7,LSR#8
            R10 = R0 & (R7 >> 8);
            //ROM:01DB89EC 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB89F0 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB89F4 4                       EOR     R10, R9, R10
            R10 = R9 ^ R10;
            //ROM:01DB89F8 4                       AND     R9, R0, R7,LSR#16
            R9 = R0 & (R7 >> 16);
            //ROM:01DB89FC 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8A00 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB8A04 4                       LDR     R7, [R2,R7,LSL#2]
            R7 = get_long((R2 + (R7 << 2)));
            //ROM:01DB8A08 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8A0C 4                       EOR     R7, R9, R7
            R7 = R9 ^ R7;
            //ROM:01DB8A10 4                       STR     R7, [R1,#0x6C]
            put_long((R1 + 0x6C), R7);
            //ROM:01DB8A14 4                       MOV     R7, R8,LSL#8
            R7 = (R8 << 8);
            //ROM:01DB8A18 4                       ORR     R7, R7, R4,LSR#24
            R7 = R7 | (R4 >> 24);
            //ROM:01DB8A1C 4                       MOV     R4, R4,LSL#8
            R4 = (R4 << 8);
            //ROM:01DB8A20 4                       ORR     R8, R4, R8,LSR#24
            R8 = R4 | (R8 >> 24);
            //ROM:01DB8A24 4                       ADD     R4, R6, R7
            R4 = R6 + R7;
            //ROM:01DB8A28 4                       ADD     R4, R4, #0x73
            R4 = R4 + 0x73;
            //ROM:01DB8A2C 4                       ADD     R4, R4, #0x19800
            R4 = R4 + 0x19800;
            //ROM:01DB8A30 4                       ADD     R4, R4, #0x1900000
            R4 = R4 + 0x1900000;
            //ROM:01DB8A34 4                       ADD     R4, R4, #0x20000000
            R4 = R4 + 0x20000000;
            //ROM:01DB8A38 4                       AND     R9, R4, #0xFF
            R9 = R4 & 0xFF;
            //ROM:01DB8A3C 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB8A40 4                       AND     R9, R0, R4,LSR#8
            R9 = R0 & (R4 >> 8);
            //ROM:01DB8A44 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8A48 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8A4C 4                       AND     R9, R0, R4,LSR#16
            R9 = R0 & (R4 >> 16);
            //ROM:01DB8A50 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8A54 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB8A58 4                       LDR     R4, [R2,R4,LSL#2]
            R4 = get_long((R2 + (R4 << 2)));
            //ROM:01DB8A5C 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8A60 4                       EOR     R4, R9, R4
            R4 = R9 ^ R4;
            //ROM:01DB8A64 4                       STR     R4, [R1,#0x70]
            put_long((R1 + 0x70), R4);
            //ROM:01DB8A68 4                       SUB     R4, R5, R8
            R4 = R5 - R8;
            //ROM:01DB8A6C 4                       SUB     R4, R4, #0x73
            R4 = R4 - 0x73;
            //ROM:01DB8A70 4                       SUB     R4, R4, #0x19800
            R4 = R4 - 0x19800;
            //ROM:01DB8A74 4                       ADD     R4, R4, #0xE700000
            R4 = R4 + 0xE700000;
            //ROM:01DB8A78 4                       ADD     R4, R4, #0xD0000000
            R4 = R4 + 0xD0000000;
            //ROM:01DB8A7C 4                       AND     R9, R4, #0xFF
            R9 = R4 & 0xFF;
            //ROM:01DB8A80 4                       LDR     R10, [LR,R9,LSL#2]
            R10 = get_long((LR + (R9 << 2)));
            //ROM:01DB8A84 4                       AND     R9, R0, R4,LSR#8
            R9 = R0 & (R4 >> 8);
            //ROM:01DB8A88 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8A8C 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8A90 4                       AND     R9, R0, R4,LSR#16
            R9 = R0 & (R4 >> 16);
            //ROM:01DB8A94 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB8A98 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB8A9C 4                       LDR     R4, [R2,R4,LSL#2]
            R4 = get_long((R2 + (R4 << 2)));
            //ROM:01DB8AA0 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8AA4 4                       EOR     R4, R9, R4
            R4 = R9 ^ R4;
            //ROM:01DB8AA8 4                       STR     R4, [R1,#0x74]
            put_long((R1 + 0x74), R4);
            //ROM:01DB8AAC 4                       MOV     R4, R6,LSR#8
            R4 = (R6 >> 8);
            //ROM:01DB8AB0 4                       ORR     R4, R4, R5,LSL#24
            R4 = R4 | (R5 << 24);
            //ROM:01DB8AB4 4                       ADD     R4, R4, R7
            R4 = R4 + R7;
            //ROM:01DB8AB8 4                       ADD     R4, R4, #0xE5
            R4 = R4 + 0xE5;
            //ROM:01DB8ABC 4                       ADD     R4, R4, #0x33000
            R4 = R4 + 0x33000;
            //ROM:01DB8AC0 4                       ADD     R4, R4, #0x3200000
            R4 = R4 + 0x3200000;
            //ROM:01DB8AC4 4                       MOV     R5, R5,LSR#8
            R5 = (R5 >> 8);
            //ROM:01DB8AC8 4                       ORR     R5, R5, R6,LSL#24
            R5 = R5 | (R6 << 24);
            //ROM:01DB8ACC 4                       ADD     R4, R4, #0x40000000
            R4 = R4 + 0x40000000;
            //ROM:01DB8AD0 4                       AND     R6, R4, #0xFF
            R6 = R4 & 0xFF;
            //ROM:01DB8AD4 4                       LDR     R7, [LR,R6,LSL#2]
            R7 = get_long((LR + (R6 << 2)));
            //ROM:01DB8AD8 4                       AND     R6, R0, R4,LSR#8
            R6 = R0 & (R4 >> 8);
            //ROM:01DB8ADC 4                       LDR     R6, [R12,R6,LSL#2]
            R6 = get_long((R12 + (R6 << 2)));
            //ROM:01DB8AE0 4                       EOR     R7, R7, R6
            R7 = R7 ^ R6;
            //ROM:01DB8AE4 4                       AND     R6, R0, R4,LSR#16
            R6 = R0 & (R4 >> 16);
            //ROM:01DB8AE8 4                       LDR     R6, [R3,R6,LSL#2]
            R6 = get_long((R3 + (R6 << 2)));
            //ROM:01DB8AEC 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB8AF0 4                       LDR     R4, [R2,R4,LSL#2]
            R4 = get_long((R2 + (R4 << 2)));
            //ROM:01DB8AF4 4                       EOR     R6, R7, R6
            R6 = R7 ^ R6;
            //ROM:01DB8AF8 4                       EOR     R4, R6, R4
            R4 = R6 ^ R4;
            //ROM:01DB8AFC 4                       STR     R4, [R1,#0x78]
            put_long((R1 + 0x78), R4);
            //ROM:01DB8B00 4                       SUB     R4, R5, R8
            R4 = R5 - R8;
            //ROM:01DB8B04 4                       SUB     R4, R4, #0xE5
            R4 = R4 - 0xE5;
            //ROM:01DB8B08 4                       ADD     R4, R4, #0xCD000
            R4 = R4 + 0xCD000;
            //ROM:01DB8B0C 4                       SUB     R4, R4, #0x3300000
            R4 = R4 - 0x3300000;
            //ROM:01DB8B10 4                       ADD     R4, R4, #0xC0000000
            R4 = R4 + 0xC0000000;
            //ROM:01DB8B14 4                       AND     R5, R4, #0xFF
            R5 = R4 & 0xFF;
            //ROM:01DB8B18 4                       LDR     LR, [LR,R5,LSL#2]
            LR = get_long((LR + (R5 << 2)));
            //ROM:01DB8B1C 4                       AND     R5, R0, R4,LSR#8
            R5 = R0 & (R4 >> 8);
            //ROM:01DB8B20 4                       AND     R0, R0, R4,LSR#16
            R0 = R0 & (R4 >> 16);
            //ROM:01DB8B24 4                       LDR     R0, [R3,R0,LSL#2]
            R0 = get_long((R3 + (R0 << 2)));
            //ROM:01DB8B28 4                       LDR     R12, [R12,R5,LSL#2]
            R12 = get_long((R12 + (R5 << 2)));
            //ROM:01DB8B2C 4                       MOV     R3, R4,LSR#24
            R3 = (R4 >> 24);
            //ROM:01DB8B30 4                       LDR     R2, [R2,R3,LSL#2]
            R2 = get_long((R2 + (R3 << 2)));
            //ROM:01DB8B34 4                       EOR     R12, LR, R12
            R12 = LR ^ R12;
            //ROM:01DB8B38 4                       EOR     R0, R12, R0
            R0 = R12 ^ R0;
            //ROM:01DB8B3C 4                       EOR     R0, R0, R2
            R0 = R0 ^ R2;
            //ROM:01DB8B40 4                       STR     R0, [R1,#0x7C]
            put_long((R1 + 0x7C), R0);
            //ROM:01DB8B44 4                       LDMFD   SP!, {R4-R10,PC}
            R4 = get_long(SP + 0x0);
            R5 = get_long(SP + 0x4);
            R6 = get_long(SP + 0x8);
            R7 = get_long(SP + 0xC);
            R8 = get_long(SP + 0x10);
            R9 = get_long(SP + 0x14);
            R10 = get_long(SP + 0x18);
            PC = get_long(SP + 0x1C);
            SP = SP + 0x20;

            //
        }

        private void Subcryptography_3()
        {
            //ROM:01DB8B58 4                       STMFD   SP!, {R4-R11,LR}
            put_long(SP - 0x4, LR);
            put_long(SP - 0x8, R11);
            put_long(SP - 0xC, R10);
            put_long(SP - 0x10, R9);
            put_long(SP - 0x14, R8);
            put_long(SP - 0x18, R7);
            put_long(SP - 0x1C, R6);
            put_long(SP - 0x20, R5);
            put_long(SP - 0x24, R4);
            SP = SP - 0x24;
            //ROM:01DB8B5C 4                       LDMIB   R0, {R3,R8}
            R3 = get_long(R0 + 0x4);
            R8 = get_long(R0 + 0x8);
            //ROM:01DB8B60 4                       LDR     R6, [R0,#0xC]
            R6 = get_long((R0 + 0xC));
            //ROM:01DB8B64 4                       LDR     R7, [R0]
            R7 = get_long(R0);
            //ROM:01DB8B68 4                       LDR     R0, [R2]
            R0 = get_long(R2);
            //ROM:01DB8B6C 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB8B70 4                       EOR     R5, R8, R0
            R5 = R8 ^ R0;
            //ROM:01DB8B74 4                       LDR     R0, [R2,#4]
            R0 = get_long((R2 + 4));
            //ROM:01DB8B78 4                       LDR     LR, =unk_224821C
            LR = 0x224821C;
            //ROM:01DB8B7C 4                       EOR     R0, R0, R6
            R0 = R0 ^ R6;
            //ROM:01DB8B80 4                       EOR     R4, R0, R5
            R4 = R0 ^ R5;
            //ROM:01DB8B84 4                       AND     R0, R4, #0xFF
            R0 = R4 & 0xFF;
            //ROM:01DB8B88 4                       LDR     R12, [R11,R0,LSL#2]
            R12 = get_long((R11 + (R0 << 2)));
            //ROM:01DB8B8C 4                       MOV     R0, #0xFF
            R0 = 0xFF;
            //ROM:01DB8B90 4                       AND     R9, R0, R4,LSR#8
            R9 = R0 & (R4 >> 8);
            //ROM:01DB8B94 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8B98 4                       AND     R10, R0, R4,LSR#16
            R10 = R0 & (R4 >> 16);
            //ROM:01DB8B9C 4                       EOR     R9, R12, R9
            R9 = R12 ^ R9;
            //ROM:01DB8BA0 4                       LDR     R12, =unk_224861C
            R12 = 0x224861C;
            //ROM:01DB8BA4 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB8BA8 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8BAC 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8BB0 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8BB4 4                       LDR     R4, [R10,R4,LSL#2]
            R4 = get_long((R10 + (R4 << 2)));
            //ROM:01DB8BB8 4                       EOR     R4, R9, R4
            R4 = R9 ^ R4;
            //ROM:01DB8BBC 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01DB8BC0 4                       AND     R9, R5, #0xFF
            R9 = R5 & 0xFF;
            //ROM:01DB8BC4 4                       AND     R10, R0, R5,LSR#8
            R10 = R0 & (R5 >> 8);
            //ROM:01DB8BC8 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB8BCC 4                       LDR     R9, [R11,R9,LSL#2]
            R9 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8BD0 4                       EOR     R10, R9, R10
            R10 = R9 ^ R10;
            //ROM:01DB8BD4 4                       AND     R9, R0, R5,LSR#16
            R9 = R0 & (R5 >> 16);
            //ROM:01DB8BD8 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8BDC 4                       MOV     R5, R5,LSR#24
            R5 = (R5 >> 24);
            //ROM:01DB8BE0 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8BE4 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8BE8 4                       LDR     R5, [R10,R5,LSL#2]
            R5 = get_long((R10 + (R5 << 2)));
            //ROM:01DB8BEC 4                       EOR     R5, R9, R5
            R5 = R9 ^ R5;
            //ROM:01DB8BF0 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01DB8BF4 4                       AND     R9, R4, #0xFF
            R9 = R4 & 0xFF;
            //ROM:01DB8BF8 4                       LDR     R10, [R11,R9,LSL#2]
            R10 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8BFC 4                       AND     R9, R0, R4,LSR#8
            R9 = R0 & (R4 >> 8);
            //ROM:01DB8C00 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8C04 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8C08 4                       AND     R10, R0, R4,LSR#16
            R10 = R0 & (R4 >> 16);
            //ROM:01DB8C0C 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8C10 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB8C14 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8C18 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8C1C 4                       LDR     R4, [R10,R4,LSL#2]
            R4 = get_long((R10 + (R4 << 2)));
            //ROM:01DB8C20 4                       EOR     R4, R9, R4
            R4 = R9 ^ R4;
            //ROM:01DB8C24 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01DB8C28 4                       EOR     R3, R3, R4
            R3 = R3 ^ R4;
            //ROM:01DB8C2C 4                       LDR     R4, [R2,#8]
            R4 = get_long((R2 + 8));
            //ROM:01DB8C30 4                       EOR     R7, R7, R5
            R7 = R7 ^ R5;
            //ROM:01DB8C34 4                       EOR     R5, R4, R7
            R5 = R4 ^ R7;
            //ROM:01DB8C38 4                       LDR     R4, [R2,#0xC]
            R4 = get_long((R2 + 0xC));
            //ROM:01DB8C3C 4                       EOR     R4, R4, R3
            R4 = R4 ^ R3;
            //ROM:01DB8C40 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01DB8C44 4                       AND     R9, R4, #0xFF
            R9 = R4 & 0xFF;
            //ROM:01DB8C48 4                       LDR     R10, [R11,R9,LSL#2]
            R10 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8C4C 4                       AND     R9, R0, R4,LSR#8
            R9 = R0 & (R4 >> 8);
            //ROM:01DB8C50 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8C54 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8C58 4                       AND     R10, R0, R4,LSR#16
            R10 = R0 & (R4 >> 16);
            //ROM:01DB8C5C 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8C60 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB8C64 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8C68 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8C6C 4                       LDR     R4, [R10,R4,LSL#2]
            R4 = get_long((R10 + (R4 << 2)));
            //ROM:01DB8C70 4                       EOR     R4, R9, R4
            R4 = R9 ^ R4;
            //ROM:01DB8C74 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01DB8C78 4                       AND     R9, R5, #0xFF
            R9 = R5 & 0xFF;
            //ROM:01DB8C7C 4                       LDR     R10, [R11,R9,LSL#2]
            R10 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8C80 4                       AND     R9, R0, R5,LSR#8
            R9 = R0 & (R5 >> 8);
            //ROM:01DB8C84 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8C88 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8C8C 4                       AND     R10, R0, R5,LSR#16
            R10 = R0 & (R5 >> 16);
            //ROM:01DB8C90 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8C94 4                       MOV     R5, R5,LSR#24
            R5 = (R5 >> 24);
            //ROM:01DB8C98 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8C9C 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8CA0 4                       LDR     R5, [R10,R5,LSL#2]
            R5 = get_long((R10 + (R5 << 2)));
            //ROM:01DB8CA4 4                       EOR     R5, R9, R5
            R5 = R9 ^ R5;
            //ROM:01DB8CA8 4                       ADD     R4, R4, R5
            R4 = R4 + R5;
            //ROM:01DB8CAC 4                       AND     R9, R4, #0xFF
            R9 = R4 & 0xFF;
            //ROM:01DB8CB0 4                       LDR     R10, [R11,R9,LSL#2]
            R10 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8CB4 4                       AND     R9, R0, R4,LSR#8
            R9 = R0 & (R4 >> 8);
            //ROM:01DB8CB8 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8CBC 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8CC0 4                       AND     R9, R0, R4,LSR#16
            R9 = R0 & (R4 >> 16);
            //ROM:01DB8CC4 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8CC8 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB8CCC 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8CD0 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8CD4 4                       LDR     R4, [R10,R4,LSL#2]
            R4 = get_long((R10 + (R4 << 2)));
            //ROM:01DB8CD8 4                       EOR     R4, R9, R4
            R4 = R9 ^ R4;
            //ROM:01DB8CDC 4                       ADD     R5, R5, R4
            R5 = R5 + R4;
            //ROM:01DB8CE0 4                       EOR     R8, R8, R5
            R8 = R8 ^ R5;
            //ROM:01DB8CE4 4                       EOR     R5, R6, R4
            R5 = R6 ^ R4;
            //ROM:01DB8CE8 4                       LDR     R4, [R2,#0x10]
            R4 = get_long((R2 + 0x10));
            //ROM:01DB8CEC 4                       EOR     R6, R4, R8
            R6 = R4 ^ R8;
            //ROM:01DB8CF0 4                       LDR     R4, [R2,#0x14]
            R4 = get_long((R2 + 0x14));
            //ROM:01DB8CF4 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01DB8CF8 4                       EOR     R4, R4, R6
            R4 = R4 ^ R6;
            //ROM:01DB8CFC 4                       AND     R9, R4, #0xFF
            R9 = R4 & 0xFF;
            //ROM:01DB8D00 4                       AND     R10, R0, R4,LSR#8
            R10 = R0 & (R4 >> 8);
            //ROM:01DB8D04 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB8D08 4                       LDR     R9, [R11,R9,LSL#2]
            R9 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8D0C 4                       EOR     R10, R9, R10
            R10 = R9 ^ R10;
            //ROM:01DB8D10 4                       AND     R9, R0, R4,LSR#16
            R9 = R0 & (R4 >> 16);
            //ROM:01DB8D14 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8D18 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB8D1C 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8D20 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8D24 4                       LDR     R4, [R10,R4,LSL#2]
            R4 = get_long((R10 + (R4 << 2)));
            //ROM:01DB8D28 4                       EOR     R4, R9, R4
            R4 = R9 ^ R4;
            //ROM:01DB8D2C 4                       ADD     R6, R6, R4
            R6 = R6 + R4;
            //ROM:01DB8D30 4                       AND     R9, R6, #0xFF
            R9 = R6 & 0xFF;
            //ROM:01DB8D34 4                       LDR     R10, [R11,R9,LSL#2]
            R10 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8D38 4                       AND     R9, R0, R6,LSR#8
            R9 = R0 & (R6 >> 8);
            //ROM:01DB8D3C 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8D40 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8D44 4                       AND     R10, R0, R6,LSR#16
            R10 = R0 & (R6 >> 16);
            //ROM:01DB8D48 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8D4C 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB8D50 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8D54 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8D58 4                       LDR     R6, [R10,R6,LSL#2]
            R6 = get_long((R10 + (R6 << 2)));
            //ROM:01DB8D5C 4                       EOR     R6, R9, R6
            R6 = R9 ^ R6;
            //ROM:01DB8D60 4                       ADD     R4, R4, R6
            R4 = R4 + R6;
            //ROM:01DB8D64 4                       AND     R9, R4, #0xFF
            R9 = R4 & 0xFF;
            //ROM:01DB8D68 4                       LDR     R10, [R11,R9,LSL#2]
            R10 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8D6C 4                       AND     R9, R0, R4,LSR#8
            R9 = R0 & (R4 >> 8);
            //ROM:01DB8D70 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8D74 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8D78 4                       AND     R10, R0, R4,LSR#16
            R10 = R0 & (R4 >> 16);
            //ROM:01DB8D7C 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8D80 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB8D84 4                       EOR     R9, R9, R10
            R9 = R9 ^ R10;
            //ROM:01DB8D88 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8D8C 4                       LDR     R4, [R10,R4,LSL#2]
            R4 = get_long((R10 + (R4 << 2)));
            //ROM:01DB8D90 4                       EOR     R4, R9, R4
            R4 = R9 ^ R4;
            //ROM:01DB8D94 4                       ADD     R6, R6, R4
            R6 = R6 + R4;
            //ROM:01DB8D98 4                       EOR     R4, R3, R4
            R4 = R3 ^ R4;
            //ROM:01DB8D9C 4                       LDR     R3, [R2,#0x18]
            R3 = get_long((R2 + 0x18));
            //ROM:01DB8DA0 4                       EOR     R6, R7, R6
            R6 = R7 ^ R6;
            //ROM:01DB8DA4 4                       EOR     R7, R3, R6
            R7 = R3 ^ R6;
            //ROM:01DB8DA8 4                       LDR     R3, [R2,#0x1C]
            R3 = get_long((R2 + 0x1C));
            //ROM:01DB8DAC 4                       EOR     R3, R3, R4
            R3 = R3 ^ R4;
            //ROM:01DB8DB0 4                       EOR     R3, R3, R7
            R3 = R3 ^ R7;
            //ROM:01DB8DB4 4                       AND     R9, R3, #0xFF
            R9 = R3 & 0xFF;
            //ROM:01DB8DB8 4                       LDR     R10, [R11,R9,LSL#2]
            R10 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8DBC 4                       AND     R9, R0, R3,LSR#8
            R9 = R0 & (R3 >> 8);
            //ROM:01DB8DC0 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8DC4 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8DC8 4                       AND     R9, R0, R3,LSR#16
            R9 = R0 & (R3 >> 16);
            //ROM:01DB8DCC 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8DD0 4                       MOV     R3, R3,LSR#24
            R3 = (R3 >> 24);
            //ROM:01DB8DD4 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8DD8 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8DDC 4                       LDR     R3, [R10,R3,LSL#2]
            R3 = get_long((R10 + (R3 << 2)));
            //ROM:01DB8DE0 4                       EOR     R3, R9, R3
            R3 = R9 ^ R3;
            //ROM:01DB8DE4 4                       ADD     R7, R7, R3
            R7 = R7 + R3;
            //ROM:01DB8DE8 4                       AND     R9, R7, #0xFF
            R9 = R7 & 0xFF;
            //ROM:01DB8DEC 4                       AND     R10, R0, R7,LSR#8
            R10 = R0 & (R7 >> 8);
            //ROM:01DB8DF0 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB8DF4 4                       LDR     R9, [R11,R9,LSL#2]
            R9 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8DF8 4                       EOR     R10, R9, R10
            R10 = R9 ^ R10;
            //ROM:01DB8DFC 4                       AND     R9, R0, R7,LSR#16
            R9 = R0 & (R7 >> 16);
            //ROM:01DB8E00 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8E04 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB8E08 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8E0C 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8E10 4                       LDR     R7, [R10,R7,LSL#2]
            R7 = get_long((R10 + (R7 << 2)));
            //ROM:01DB8E14 4                       EOR     R7, R9, R7
            R7 = R9 ^ R7;
            //ROM:01DB8E18 4                       ADD     R3, R3, R7
            R3 = R3 + R7;
            //ROM:01DB8E1C 4                       AND     R9, R3, #0xFF
            R9 = R3 & 0xFF;
            //ROM:01DB8E20 4                       LDR     R10, [R11,R9,LSL#2]
            R10 = get_long((R11 + (R9 << 2)));
            //ROM:01DB8E24 4                       AND     R9, R0, R3,LSR#8
            R9 = R0 & (R3 >> 8);
            //ROM:01DB8E28 4                       LDR     R9, [LR,R9,LSL#2]
            R9 = get_long((LR + (R9 << 2)));
            //ROM:01DB8E2C 4                       EOR     R10, R10, R9
            R10 = R10 ^ R9;
            //ROM:01DB8E30 4                       AND     R9, R0, R3,LSR#16
            R9 = R0 & (R3 >> 16);
            //ROM:01DB8E34 4                       LDR     R9, [R12,R9,LSL#2]
            R9 = get_long((R12 + (R9 << 2)));
            //ROM:01DB8E38 4                       MOV     R3, R3,LSR#24
            R3 = (R3 >> 24);
            //ROM:01DB8E3C 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB8E40 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8E44 4                       LDR     R3, [R10,R3,LSL#2]
            R3 = get_long((R10 + (R3 << 2)));
            //ROM:01DB8E48 4                       EOR     R3, R9, R3
            R3 = R9 ^ R3;
            //ROM:01DB8E4C 4                       ADD     R7, R7, R3
            R7 = R7 + R3;
            //ROM:01DB8E50 4                       EOR     R9, R8, R7
            R9 = R8 ^ R7;
            //ROM:01DB8E54 4                       EOR     R7, R5, R3
            R7 = R5 ^ R3;
            //ROM:01DB8E58 4                       LDR     R3, [R2,#0x20]
            R3 = get_long((R2 + 0x20));
            //ROM:01DB8E5C 4                       EOR     R5, R3, R9
            R5 = R3 ^ R9;
            //ROM:01DB8E60 4                       LDR     R3, [R2,#0x24]
            R3 = get_long((R2 + 0x24));
            //ROM:01DB8E64 4                       EOR     R3, R3, R7
            R3 = R3 ^ R7;
            //ROM:01DB8E68 4                       EOR     R3, R3, R5
            R3 = R3 ^ R5;
            //ROM:01DB8E6C 4                       AND     R8, R3, #0xFF
            R8 = R3 & 0xFF;
            //ROM:01DB8E70 4                       LDR     R8, [R11,R8,LSL#2]
            R8 = get_long((R11 + (R8 << 2)));
            //ROM:01DB8E74 4                       AND     R10, R0, R3,LSR#8
            R10 = R0 & (R3 >> 8);
            //ROM:01DB8E78 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB8E7C 4                       EOR     R8, R8, R10
            R8 = R8 ^ R10;
            //ROM:01DB8E80 4                       AND     R10, R0, R3,LSR#16
            R10 = R0 & (R3 >> 16);
            //ROM:01DB8E84 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8E88 4                       MOV     R3, R3,LSR#24
            R3 = (R3 >> 24);
            //ROM:01DB8E8C 4                       EOR     R8, R8, R10
            R8 = R8 ^ R10;
            //ROM:01DB8E90 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8E94 4                       LDR     R3, [R10,R3,LSL#2]
            R3 = get_long((R10 + (R3 << 2)));
            //ROM:01DB8E98 4                       EOR     R3, R8, R3
            R3 = R8 ^ R3;
            //ROM:01DB8E9C 4                       ADD     R5, R5, R3
            R5 = R5 + R3;
            //ROM:01DB8EA0 4                       AND     R8, R5, #0xFF
            R8 = R5 & 0xFF;
            //ROM:01DB8EA4 4                       AND     R10, R0, R5,LSR#8
            R10 = R0 & (R5 >> 8);
            //ROM:01DB8EA8 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB8EAC 4                       LDR     R8, [R11,R8,LSL#2]
            R8 = get_long((R11 + (R8 << 2)));
            //ROM:01DB8EB0 4                       EOR     R8, R8, R10
            R8 = R8 ^ R10;
            //ROM:01DB8EB4 4                       AND     R10, R0, R5,LSR#16
            R10 = R0 & (R5 >> 16);
            //ROM:01DB8EB8 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8EBC 4                       MOV     R5, R5,LSR#24
            R5 = (R5 >> 24);
            //ROM:01DB8EC0 4                       EOR     R8, R8, R10
            R8 = R8 ^ R10;
            //ROM:01DB8EC4 4                       LDR     R10, =unk_2248A1C
            R10 = 0x2248A1C;
            //ROM:01DB8EC8 4                       LDR     R5, [R10,R5,LSL#2]
            R5 = get_long((R10 + (R5 << 2)));
            //ROM:01DB8ECC 4                       MOV     R10, #0xFF
            R10 = 0xFF;
            //ROM:01DB8ED0 4                       EOR     R5, R8, R5
            R5 = R8 ^ R5;
            //ROM:01DB8ED4 4                       ADD     R8, R3, R5
            R8 = R3 + R5;
            //ROM:01DB8ED8 4                       AND     R3, R8, #0xFF
            R3 = R8 & 0xFF;
            //ROM:01DB8EDC 4                       AND     R10, R10, R8,LSR#8
            R10 = R10 & (R8 >> 8);
            //ROM:01DB8EE0 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB8EE4 4                       LDR     R3, [R11,R3,LSL#2]
            R3 = get_long((R11 + (R3 << 2)));
            //ROM:01DB8EE8 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB8EEC 4                       EOR     R3, R3, R10
            R3 = R3 ^ R10;
            //ROM:01DB8EF0 4                       AND     R10, R0, R8,LSR#16
            R10 = R0 & (R8 >> 16);
            //ROM:01DB8EF4 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8EF8 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB8EFC 4                       EOR     R10, R3, R10
            R10 = R3 ^ R10;
            //ROM:01DB8F00 4                       LDR     R3, =unk_2248A1C
            R3 = 0x2248A1C;
            //ROM:01DB8F04 4                       LDR     R8, [R3,R8,LSL#2]
            R8 = get_long((R3 + (R8 << 2)));
            //ROM:01DB8F08 4                       EOR     R10, R10, R8
            R10 = R10 ^ R8;
            //ROM:01DB8F0C 4                       ADD     R5, R5, R10
            R5 = R5 + R10;
            //ROM:01DB8F10 4                       EOR     R8, R6, R5
            R8 = R6 ^ R5;
            //ROM:01DB8F14 4                       LDR     R5, [R2,#0x28]
            R5 = get_long((R2 + 0x28));
            //ROM:01DB8F18 4                       EOR     R4, R4, R10
            R4 = R4 ^ R10;
            //ROM:01DB8F1C 4                       EOR     R6, R5, R8
            R6 = R5 ^ R8;
            //ROM:01DB8F20 4                       LDR     R5, [R2,#0x2C]
            R5 = get_long((R2 + 0x2C));
            //ROM:01DB8F24 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01DB8F28 4                       EOR     R5, R5, R6
            R5 = R5 ^ R6;
            //ROM:01DB8F2C 4                       AND     R10, R5, #0xFF
            R10 = R5 & 0xFF;
            //ROM:01DB8F30 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB8F34 4                       AND     R10, R0, R5,LSR#8
            R10 = R0 & (R5 >> 8);
            //ROM:01DB8F38 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB8F3C 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB8F40 4                       AND     R11, R0, R5,LSR#16
            R11 = R0 & (R5 >> 16);
            //ROM:01DB8F44 4                       LDR     R11, [R12,R11,LSL#2]
            R11 = get_long((R12 + (R11 << 2)));
            //ROM:01DB8F48 4                       MOV     R5, R5,LSR#24
            R5 = (R5 >> 24);
            //ROM:01DB8F4C 4                       LDR     R5, [R3,R5,LSL#2]
            R5 = get_long((R3 + (R5 << 2)));
            //ROM:01DB8F50 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB8F54 4                       EOR     R5, R10, R5
            R5 = R10 ^ R5;
            //ROM:01DB8F58 4                       ADD     R6, R6, R5
            R6 = R6 + R5;
            //ROM:01DB8F5C 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB8F60 4                       AND     R10, R6, #0xFF
            R10 = R6 & 0xFF;
            //ROM:01DB8F64 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB8F68 4                       AND     R10, R0, R6,LSR#8
            R10 = R0 & (R6 >> 8);
            //ROM:01DB8F6C 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB8F70 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB8F74 4                       AND     R10, R0, R6,LSR#16
            R10 = R0 & (R6 >> 16);
            //ROM:01DB8F78 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB8F7C 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB8F80 4                       LDR     R6, [R3,R6,LSL#2]
            R6 = get_long((R3 + (R6 << 2)));
            //ROM:01DB8F84 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB8F88 4                       EOR     R6, R10, R6
            R6 = R10 ^ R6;
            //ROM:01DB8F8C 4                       ADD     R5, R5, R6
            R5 = R5 + R6;
            //ROM:01DB8F90 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB8F94 4                       AND     R10, R5, #0xFF
            R10 = R5 & 0xFF;
            //ROM:01DB8F98 4                       LDR     R10, [R11,R10,LSL#2]
            R10 = get_long((R11 + (R10 << 2)));
            //ROM:01DB8F9C 4                       AND     R11, R0, R5,LSR#8
            R11 = R0 & (R5 >> 8);
            //ROM:01DB8FA0 4                       LDR     R11, [LR,R11,LSL#2]
            R11 = get_long((LR + (R11 << 2)));
            //ROM:01DB8FA4 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB8FA8 4                       AND     R11, R0, R5,LSR#16
            R11 = R0 & (R5 >> 16);
            //ROM:01DB8FAC 4                       LDR     R11, [R12,R11,LSL#2]
            R11 = get_long((R12 + (R11 << 2)));
            //ROM:01DB8FB0 4                       MOV     R5, R5,LSR#24
            R5 = (R5 >> 24);
            //ROM:01DB8FB4 4                       LDR     R5, [R3,R5,LSL#2]
            R5 = get_long((R3 + (R5 << 2)));
            //ROM:01DB8FB8 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB8FBC 4                       EOR     R5, R10, R5
            R5 = R10 ^ R5;
            //ROM:01DB8FC0 4                       ADD     R6, R6, R5
            R6 = R6 + R5;
            //ROM:01DB8FC4 4                       EOR     R9, R9, R6
            R9 = R9 ^ R6;
            //ROM:01DB8FC8 4                       LDR     R6, [R2,#0x30]
            R6 = get_long((R2 + 0x30));
            //ROM:01DB8FCC 4                       EOR     R5, R7, R5
            R5 = R7 ^ R5;
            //ROM:01DB8FD0 4                       EOR     R7, R6, R9
            R7 = R6 ^ R9;
            //ROM:01DB8FD4 4                       LDR     R6, [R2,#0x34]
            R6 = get_long((R2 + 0x34));
            //ROM:01DB8FD8 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB8FDC 4                       EOR     R6, R6, R5
            R6 = R6 ^ R5;
            //ROM:01DB8FE0 4                       EOR     R6, R6, R7
            R6 = R6 ^ R7;
            //ROM:01DB8FE4 4                       AND     R10, R6, #0xFF
            R10 = R6 & 0xFF;
            //ROM:01DB8FE8 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB8FEC 4                       AND     R10, R0, R6,LSR#8
            R10 = R0 & (R6 >> 8);
            //ROM:01DB8FF0 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB8FF4 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB8FF8 4                       AND     R10, R0, R6,LSR#16
            R10 = R0 & (R6 >> 16);
            //ROM:01DB8FFC 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9000 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB9004 4                       LDR     R6, [R3,R6,LSL#2]
            R6 = get_long((R3 + (R6 << 2)));
            //ROM:01DB9008 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB900C 4                       EOR     R6, R10, R6
            R6 = R10 ^ R6;
            //ROM:01DB9010 4                       ADD     R7, R7, R6
            R7 = R7 + R6;
            //ROM:01DB9014 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9018 4                       AND     R10, R7, #0xFF
            R10 = R7 & 0xFF;
            //ROM:01DB901C 4                       LDR     R10, [R11,R10,LSL#2]
            R10 = get_long((R11 + (R10 << 2)));
            //ROM:01DB9020 4                       AND     R11, R0, R7,LSR#8
            R11 = R0 & (R7 >> 8);
            //ROM:01DB9024 4                       LDR     R11, [LR,R11,LSL#2]
            R11 = get_long((LR + (R11 << 2)));
            //ROM:01DB9028 4                       EOR     R11, R10, R11
            R11 = R10 ^ R11;
            //ROM:01DB902C 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB9030 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9034 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB9038 4                       LDR     R7, [R3,R7,LSL#2]
            R7 = get_long((R3 + (R7 << 2)));
            //ROM:01DB903C 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9040 4                       EOR     R7, R10, R7
            R7 = R10 ^ R7;
            //ROM:01DB9044 4                       ADD     R6, R6, R7
            R6 = R6 + R7;
            //ROM:01DB9048 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB904C 4                       AND     R10, R6, #0xFF
            R10 = R6 & 0xFF;
            //ROM:01DB9050 4                       LDR     R10, [R11,R10,LSL#2]
            R10 = get_long((R11 + (R10 << 2)));
            //ROM:01DB9054 4                       AND     R11, R0, R6,LSR#8
            R11 = R0 & (R6 >> 8);
            //ROM:01DB9058 4                       LDR     R11, [LR,R11,LSL#2]
            R11 = get_long((LR + (R11 << 2)));
            //ROM:01DB905C 4                       EOR     R11, R10, R11
            R11 = R10 ^ R11;
            //ROM:01DB9060 4                       AND     R10, R0, R6,LSR#16
            R10 = R0 & (R6 >> 16);
            //ROM:01DB9064 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9068 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB906C 4                       LDR     R6, [R3,R6,LSL#2]
            R6 = get_long((R3 + (R6 << 2)));
            //ROM:01DB9070 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9074 4                       EOR     R6, R10, R6
            R6 = R10 ^ R6;
            //ROM:01DB9078 4                       ADD     R7, R7, R6
            R7 = R7 + R6;
            //ROM:01DB907C 4                       EOR     R6, R4, R6
            R6 = R4 ^ R6;
            //ROM:01DB9080 4                       LDR     R4, [R2,#0x38]
            R4 = get_long((R2 + 0x38));
            //ROM:01DB9084 4                       EOR     R7, R8, R7
            R7 = R8 ^ R7;
            //ROM:01DB9088 4                       EOR     R8, R4, R7
            R8 = R4 ^ R7;
            //ROM:01DB908C 4                       LDR     R4, [R2,#0x3C]
            R4 = get_long((R2 + 0x3C));
            //ROM:01DB9090 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9094 4                       EOR     R4, R4, R6
            R4 = R4 ^ R6;
            //ROM:01DB9098 4                       EOR     R4, R4, R8
            R4 = R4 ^ R8;
            //ROM:01DB909C 4                       AND     R10, R4, #0xFF
            R10 = R4 & 0xFF;
            //ROM:01DB90A0 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB90A4 4                       AND     R10, R0, R4,LSR#8
            R10 = R0 & (R4 >> 8);
            //ROM:01DB90A8 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB90AC 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB90B0 4                       AND     R10, R0, R4,LSR#16
            R10 = R0 & (R4 >> 16);
            //ROM:01DB90B4 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB90B8 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB90BC 4                       LDR     R4, [R3,R4,LSL#2]
            R4 = get_long((R3 + (R4 << 2)));
            //ROM:01DB90C0 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB90C4 4                       EOR     R4, R10, R4
            R4 = R10 ^ R4;
            //ROM:01DB90C8 4                       ADD     R8, R8, R4
            R8 = R8 + R4;
            //ROM:01DB90CC 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB90D0 4                       AND     R10, R8, #0xFF
            R10 = R8 & 0xFF;
            //ROM:01DB90D4 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB90D8 4                       AND     R10, R0, R8,LSR#8
            R10 = R0 & (R8 >> 8);
            //ROM:01DB90DC 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB90E0 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB90E4 4                       AND     R10, R0, R8,LSR#16
            R10 = R0 & (R8 >> 16);
            //ROM:01DB90E8 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB90EC 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB90F0 4                       LDR     R8, [R3,R8,LSL#2]
            R8 = get_long((R3 + (R8 << 2)));
            //ROM:01DB90F4 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB90F8 4                       EOR     R8, R10, R8
            R8 = R10 ^ R8;
            //ROM:01DB90FC 4                       ADD     R4, R4, R8
            R4 = R4 + R8;
            //ROM:01DB9100 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9104 4                       AND     R10, R4, #0xFF
            R10 = R4 & 0xFF;
            //ROM:01DB9108 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB910C 4                       AND     R10, R0, R4,LSR#8
            R10 = R0 & (R4 >> 8);
            //ROM:01DB9110 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB9114 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB9118 4                       AND     R10, R0, R4,LSR#16
            R10 = R0 & (R4 >> 16);
            //ROM:01DB911C 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9120 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB9124 4                       LDR     R4, [R3,R4,LSL#2]
            R4 = get_long((R3 + (R4 << 2)));
            //ROM:01DB9128 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB912C 4                       EOR     R4, R10, R4
            R4 = R10 ^ R4;
            //ROM:01DB9130 4                       ADD     R8, R8, R4
            R8 = R8 + R4;
            //ROM:01DB9134 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB9138 4                       EOR     R5, R5, R4
            R5 = R5 ^ R4;
            //ROM:01DB913C 4                       LDR     R4, [R2,#0x40]
            R4 = get_long((R2 + 0x40));
            //ROM:01DB9140 4                       LDR     R9, [R2,#0x44]
            R9 = get_long((R2 + 0x44));
            //ROM:01DB9144 4                       EOR     R4, R4, R8
            R4 = R4 ^ R8;
            //ROM:01DB9148 4                       EOR     R9, R9, R5
            R9 = R9 ^ R5;
            //ROM:01DB914C 4                       EOR     R9, R9, R4
            R9 = R9 ^ R4;
            //ROM:01DB9150 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9154 4                       AND     R10, R9, #0xFF
            R10 = R9 & 0xFF;
            //ROM:01DB9158 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB915C 4                       AND     R10, R0, R9,LSR#8
            R10 = R0 & (R9 >> 8);
            //ROM:01DB9160 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB9164 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9168 4                       AND     R11, R0, R9,LSR#16
            R11 = R0 & (R9 >> 16);
            //ROM:01DB916C 4                       LDR     R11, [R12,R11,LSL#2]
            R11 = get_long((R12 + (R11 << 2)));
            //ROM:01DB9170 4                       MOV     R9, R9,LSR#24
            R9 = (R9 >> 24);
            //ROM:01DB9174 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB9178 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB917C 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB9180 4                       ADD     R4, R4, R9
            R4 = R4 + R9;
            //ROM:01DB9184 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9188 4                       AND     R10, R4, #0xFF
            R10 = R4 & 0xFF;
            //ROM:01DB918C 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB9190 4                       AND     R10, R0, R4,LSR#8
            R10 = R0 & (R4 >> 8);
            //ROM:01DB9194 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB9198 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB919C 4                       AND     R10, R0, R4,LSR#16
            R10 = R0 & (R4 >> 16);
            //ROM:01DB91A0 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB91A4 4                       MOV     R4, R4,LSR#24
            R4 = (R4 >> 24);
            //ROM:01DB91A8 4                       LDR     R4, [R3,R4,LSL#2]
            R4 = get_long((R3 + (R4 << 2)));
            //ROM:01DB91AC 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB91B0 4                       EOR     R4, R10, R4
            R4 = R10 ^ R4;
            //ROM:01DB91B4 4                       ADD     R9, R9, R4
            R9 = R9 + R4;
            //ROM:01DB91B8 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB91BC 4                       AND     R10, R9, #0xFF
            R10 = R9 & 0xFF;
            //ROM:01DB91C0 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB91C4 4                       AND     R10, R0, R9,LSR#8
            R10 = R0 & (R9 >> 8);
            //ROM:01DB91C8 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB91CC 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB91D0 4                       AND     R11, R0, R9,LSR#16
            R11 = R0 & (R9 >> 16);
            //ROM:01DB91D4 4                       LDR     R11, [R12,R11,LSL#2]
            R11 = get_long((R12 + (R11 << 2)));
            //ROM:01DB91D8 4                       MOV     R9, R9,LSR#24
            R9 = (R9 >> 24);
            //ROM:01DB91DC 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB91E0 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB91E4 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB91E8 4                       ADD     R4, R4, R9
            R4 = R4 + R9;
            //ROM:01DB91EC 4                       EOR     R4, R7, R4
            R4 = R7 ^ R4;
            //ROM:01DB91F0 4                       LDR     R7, [R2,#0x48]
            R7 = get_long((R2 + 0x48));
            //ROM:01DB91F4 4                       EOR     R6, R6, R9
            R6 = R6 ^ R9;
            //ROM:01DB91F8 4                       EOR     R9, R7, R4
            R9 = R7 ^ R4;
            //ROM:01DB91FC 4                       LDR     R7, [R2,#0x4C]
            R7 = get_long((R2 + 0x4C));
            //ROM:01DB9200 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9204 4                       EOR     R7, R7, R6
            R7 = R7 ^ R6;
            //ROM:01DB9208 4                       EOR     R7, R7, R9
            R7 = R7 ^ R9;
            //ROM:01DB920C 4                       AND     R10, R7, #0xFF
            R10 = R7 & 0xFF;
            //ROM:01DB9210 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB9214 4                       AND     R10, R0, R7,LSR#8
            R10 = R0 & (R7 >> 8);
            //ROM:01DB9218 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB921C 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB9220 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB9224 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9228 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB922C 4                       LDR     R7, [R3,R7,LSL#2]
            R7 = get_long((R3 + (R7 << 2)));
            //ROM:01DB9230 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9234 4                       EOR     R7, R10, R7
            R7 = R10 ^ R7;
            //ROM:01DB9238 4                       ADD     R9, R9, R7
            R9 = R9 + R7;
            //ROM:01DB923C 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9240 4                       AND     R10, R9, #0xFF
            R10 = R9 & 0xFF;
            //ROM:01DB9244 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB9248 4                       AND     R10, R0, R9,LSR#8
            R10 = R0 & (R9 >> 8);
            //ROM:01DB924C 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB9250 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9254 4                       AND     R11, R0, R9,LSR#16
            R11 = R0 & (R9 >> 16);
            //ROM:01DB9258 4                       LDR     R11, [R12,R11,LSL#2]
            R11 = get_long((R12 + (R11 << 2)));
            //ROM:01DB925C 4                       MOV     R9, R9,LSR#24
            R9 = (R9 >> 24);
            //ROM:01DB9260 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB9264 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB9268 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB926C 4                       ADD     R7, R7, R9
            R7 = R7 + R9;
            //ROM:01DB9270 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9274 4                       AND     R10, R7, #0xFF
            R10 = R7 & 0xFF;
            //ROM:01DB9278 4                       LDR     R10, [R11,R10,LSL#2]
            R10 = get_long((R11 + (R10 << 2)));
            //ROM:01DB927C 4                       AND     R11, R0, R7,LSR#8
            R11 = R0 & (R7 >> 8);
            //ROM:01DB9280 4                       LDR     R11, [LR,R11,LSL#2]
            R11 = get_long((LR + (R11 << 2)));
            //ROM:01DB9284 4                       EOR     R11, R10, R11
            R11 = R10 ^ R11;
            //ROM:01DB9288 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB928C 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9290 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB9294 4                       LDR     R7, [R3,R7,LSL#2]
            R7 = get_long((R3 + (R7 << 2)));
            //ROM:01DB9298 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB929C 4                       EOR     R10, R10, R7
            R10 = R10 ^ R7;
            //ROM:01DB92A0 4                       ADD     R7, R9, R10
            R7 = R9 + R10;
            //ROM:01DB92A4 4                       EOR     R7, R8, R7
            R7 = R8 ^ R7;
            //ROM:01DB92A8 4                       LDR     R8, [R2,#0x50]
            R8 = get_long((R2 + 0x50));
            //ROM:01DB92AC 4                       LDR     R9, [R2,#0x54]
            R9 = get_long((R2 + 0x54));
            //ROM:01DB92B0 4                       EOR     R5, R5, R10
            R5 = R5 ^ R10;
            //ROM:01DB92B4 4                       EOR     R9, R9, R5
            R9 = R9 ^ R5;
            //ROM:01DB92B8 4                       EOR     R8, R8, R7
            R8 = R8 ^ R7;
            //ROM:01DB92BC 4                       EOR     R9, R9, R8
            R9 = R9 ^ R8;
            //ROM:01DB92C0 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB92C4 4                       AND     R10, R9, #0xFF
            R10 = R9 & 0xFF;
            //ROM:01DB92C8 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB92CC 4                       AND     R10, R0, R9,LSR#8
            R10 = R0 & (R9 >> 8);
            //ROM:01DB92D0 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB92D4 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB92D8 4                       AND     R11, R0, R9,LSR#16
            R11 = R0 & (R9 >> 16);
            //ROM:01DB92DC 4                       LDR     R11, [R12,R11,LSL#2]
            R11 = get_long((R12 + (R11 << 2)));
            //ROM:01DB92E0 4                       MOV     R9, R9,LSR#24
            R9 = (R9 >> 24);
            //ROM:01DB92E4 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB92E8 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB92EC 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB92F0 4                       ADD     R8, R8, R9
            R8 = R8 + R9;
            //ROM:01DB92F4 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB92F8 4                       AND     R10, R8, #0xFF
            R10 = R8 & 0xFF;
            //ROM:01DB92FC 4                       LDR     R10, [R11,R10,LSL#2]
            R10 = get_long((R11 + (R10 << 2)));
            //ROM:01DB9300 4                       AND     R11, R0, R8,LSR#8
            R11 = R0 & (R8 >> 8);
            //ROM:01DB9304 4                       LDR     R11, [LR,R11,LSL#2]
            R11 = get_long((LR + (R11 << 2)));
            //ROM:01DB9308 4                       EOR     R11, R10, R11
            R11 = R10 ^ R11;
            //ROM:01DB930C 4                       AND     R10, R0, R8,LSR#16
            R10 = R0 & (R8 >> 16);
            //ROM:01DB9310 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9314 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB9318 4                       LDR     R8, [R3,R8,LSL#2]
            R8 = get_long((R3 + (R8 << 2)));
            //ROM:01DB931C 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9320 4                       EOR     R8, R10, R8
            R8 = R10 ^ R8;
            //ROM:01DB9324 4                       ADD     R9, R9, R8
            R9 = R9 + R8;
            //ROM:01DB9328 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB932C 4                       AND     R10, R9, #0xFF
            R10 = R9 & 0xFF;
            //ROM:01DB9330 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB9334 4                       AND     R10, R0, R9,LSR#8
            R10 = R0 & (R9 >> 8);
            //ROM:01DB9338 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB933C 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9340 4                       AND     R11, R0, R9,LSR#16
            R11 = R0 & (R9 >> 16);
            //ROM:01DB9344 4                       LDR     R11, [R12,R11,LSL#2]
            R11 = get_long((R12 + (R11 << 2)));
            //ROM:01DB9348 4                       MOV     R9, R9,LSR#24
            R9 = (R9 >> 24);
            //ROM:01DB934C 4                       LDR     R9, [R3,R9,LSL#2]
            R9 = get_long((R3 + (R9 << 2)));
            //ROM:01DB9350 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB9354 4                       EOR     R9, R10, R9
            R9 = R10 ^ R9;
            //ROM:01DB9358 4                       ADD     R8, R8, R9
            R8 = R8 + R9;
            //ROM:01DB935C 4                       EOR     R4, R4, R8
            R4 = R4 ^ R8;
            //ROM:01DB9360 4                       EOR     R9, R6, R9
            R9 = R6 ^ R9;
            //ROM:01DB9364 4                       LDR     R6, [R2,#0x58]
            R6 = get_long((R2 + 0x58));
            //ROM:01DB9368 4                       LDR     R8, [R2,#0x5C]
            R8 = get_long((R2 + 0x5C));
            //ROM:01DB936C 4                       EOR     R6, R6, R4
            R6 = R6 ^ R4;
            //ROM:01DB9370 4                       EOR     R8, R8, R9
            R8 = R8 ^ R9;
            //ROM:01DB9374 4                       EOR     R8, R8, R6
            R8 = R8 ^ R6;
            //ROM:01DB9378 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB937C 4                       AND     R10, R8, #0xFF
            R10 = R8 & 0xFF;
            //ROM:01DB9380 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB9384 4                       AND     R10, R0, R8,LSR#8
            R10 = R0 & (R8 >> 8);
            //ROM:01DB9388 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB938C 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9390 4                       AND     R11, R0, R8,LSR#16
            R11 = R0 & (R8 >> 16);
            //ROM:01DB9394 4                       LDR     R11, [R12,R11,LSL#2]
            R11 = get_long((R12 + (R11 << 2)));
            //ROM:01DB9398 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB939C 4                       LDR     R8, [R3,R8,LSL#2]
            R8 = get_long((R3 + (R8 << 2)));
            //ROM:01DB93A0 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB93A4 4                       EOR     R8, R10, R8
            R8 = R10 ^ R8;
            //ROM:01DB93A8 4                       ADD     R6, R6, R8
            R6 = R6 + R8;
            //ROM:01DB93AC 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB93B0 4                       AND     R10, R6, #0xFF
            R10 = R6 & 0xFF;
            //ROM:01DB93B4 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB93B8 4                       AND     R10, R0, R6,LSR#8
            R10 = R0 & (R6 >> 8);
            //ROM:01DB93BC 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB93C0 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB93C4 4                       AND     R10, R0, R6,LSR#16
            R10 = R0 & (R6 >> 16);
            //ROM:01DB93C8 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB93CC 4                       MOV     R6, R6,LSR#24
            R6 = (R6 >> 24);
            //ROM:01DB93D0 4                       LDR     R6, [R3,R6,LSL#2]
            R6 = get_long((R3 + (R6 << 2)));
            //ROM:01DB93D4 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB93D8 4                       EOR     R6, R10, R6
            R6 = R10 ^ R6;
            //ROM:01DB93DC 4                       ADD     R8, R8, R6
            R8 = R8 + R6;
            //ROM:01DB93E0 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB93E4 4                       AND     R10, R8, #0xFF
            R10 = R8 & 0xFF;
            //ROM:01DB93E8 4                       LDR     R10, [R11,R10,LSL#2]
            R10 = get_long((R11 + (R10 << 2)));
            //ROM:01DB93EC 4                       AND     R11, R0, R8,LSR#8
            R11 = R0 & (R8 >> 8);
            //ROM:01DB93F0 4                       LDR     R11, [LR,R11,LSL#2]
            R11 = get_long((LR + (R11 << 2)));
            //ROM:01DB93F4 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB93F8 4                       AND     R11, R0, R8,LSR#16
            R11 = R0 & (R8 >> 16);
            //ROM:01DB93FC 4                       LDR     R11, [R12,R11,LSL#2]
            R11 = get_long((R12 + (R11 << 2)));
            //ROM:01DB9400 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB9404 4                       LDR     R8, [R3,R8,LSL#2]
            R8 = get_long((R3 + (R8 << 2)));
            //ROM:01DB9408 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB940C 4                       EOR     R8, R10, R8
            R8 = R10 ^ R8;
            //ROM:01DB9410 4                       ADD     R6, R6, R8
            R6 = R6 + R8;
            //ROM:01DB9414 4                       EOR     R6, R7, R6
            R6 = R7 ^ R6;
            //ROM:01DB9418 4                       EOR     R5, R5, R8
            R5 = R5 ^ R8;
            //ROM:01DB941C 4                       LDR     R8, [R2,#0x64]
            R8 = get_long((R2 + 0x64));
            //ROM:01DB9420 4                       LDR     R7, [R2,#0x60]
            R7 = get_long((R2 + 0x60));
            //ROM:01DB9424 4                       EOR     R8, R8, R5
            R8 = R8 ^ R5;
            //ROM:01DB9428 4                       EOR     R7, R7, R6
            R7 = R7 ^ R6;
            //ROM:01DB942C 4                       EOR     R8, R8, R7
            R8 = R8 ^ R7;
            //ROM:01DB9430 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9434 4                       AND     R10, R8, #0xFF
            R10 = R8 & 0xFF;
            //ROM:01DB9438 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB943C 4                       AND     R10, R0, R8,LSR#8
            R10 = R0 & (R8 >> 8);
            //ROM:01DB9440 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB9444 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB9448 4                       AND     R10, R0, R8,LSR#16
            R10 = R0 & (R8 >> 16);
            //ROM:01DB944C 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9450 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB9454 4                       LDR     R8, [R3,R8,LSL#2]
            R8 = get_long((R3 + (R8 << 2)));
            //ROM:01DB9458 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB945C 4                       EOR     R8, R10, R8
            R8 = R10 ^ R8;
            //ROM:01DB9460 4                       ADD     R7, R7, R8
            R7 = R7 + R8;
            //ROM:01DB9464 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9468 4                       AND     R10, R7, #0xFF
            R10 = R7 & 0xFF;
            //ROM:01DB946C 4                       LDR     R10, [R11,R10,LSL#2]
            R10 = get_long((R11 + (R10 << 2)));
            //ROM:01DB9470 4                       AND     R11, R0, R7,LSR#8
            R11 = R0 & (R7 >> 8);
            //ROM:01DB9474 4                       LDR     R11, [LR,R11,LSL#2]
            R11 = get_long((LR + (R11 << 2)));
            //ROM:01DB9478 4                       EOR     R11, R10, R11
            R11 = R10 ^ R11;
            //ROM:01DB947C 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB9480 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9484 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB9488 4                       LDR     R7, [R3,R7,LSL#2]
            R7 = get_long((R3 + (R7 << 2)));
            //ROM:01DB948C 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9490 4                       EOR     R7, R10, R7
            R7 = R10 ^ R7;
            //ROM:01DB9494 4                       ADD     R8, R8, R7
            R8 = R8 + R7;
            //ROM:01DB9498 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB949C 4                       AND     R10, R8, #0xFF
            R10 = R8 & 0xFF;
            //ROM:01DB94A0 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB94A4 4                       AND     R10, R0, R8,LSR#8
            R10 = R0 & (R8 >> 8);
            //ROM:01DB94A8 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB94AC 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB94B0 4                       AND     R10, R0, R8,LSR#16
            R10 = R0 & (R8 >> 16);
            //ROM:01DB94B4 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB94B8 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB94BC 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB94C0 4                       LDR     R8, [R3,R8,LSL#2]
            R8 = get_long((R3 + (R8 << 2)));
            //ROM:01DB94C4 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB94C8 4                       EOR     R8, R10, R8
            R8 = R10 ^ R8;
            //ROM:01DB94CC 4                       ADD     R7, R7, R8
            R7 = R7 + R8;
            //ROM:01DB94D0 4                       EOR     R4, R4, R7
            R4 = R4 ^ R7;
            //ROM:01DB94D4 4                       LDR     R7, [R2,#0x68]
            R7 = get_long((R2 + 0x68));
            //ROM:01DB94D8 4                       EOR     R9, R9, R8
            R9 = R9 ^ R8;
            //ROM:01DB94DC 4                       EOR     R8, R7, R4
            R8 = R7 ^ R4;
            //ROM:01DB94E0 4                       LDR     R7, [R2,#0x6C]
            R7 = get_long((R2 + 0x6C));
            //ROM:01DB94E4 4                       EOR     R7, R7, R9
            R7 = R7 ^ R9;
            //ROM:01DB94E8 4                       EOR     R7, R7, R8
            R7 = R7 ^ R8;
            //ROM:01DB94EC 4                       AND     R10, R7, #0xFF
            R10 = R7 & 0xFF;
            //ROM:01DB94F0 4                       LDR     R10, [R11,R10,LSL#2]
            R10 = get_long((R11 + (R10 << 2)));
            //ROM:01DB94F4 4                       AND     R11, R0, R7,LSR#8
            R11 = R0 & (R7 >> 8);
            //ROM:01DB94F8 4                       LDR     R11, [LR,R11,LSL#2]
            R11 = get_long((LR + (R11 << 2)));
            //ROM:01DB94FC 4                       EOR     R11, R10, R11
            R11 = R10 ^ R11;
            //ROM:01DB9500 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB9504 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9508 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB950C 4                       LDR     R7, [R3,R7,LSL#2]
            R7 = get_long((R3 + (R7 << 2)));
            //ROM:01DB9510 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9514 4                       EOR     R7, R10, R7
            R7 = R10 ^ R7;
            //ROM:01DB9518 4                       ADD     R8, R8, R7
            R8 = R8 + R7;
            //ROM:01DB951C 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9520 4                       AND     R10, R8, #0xFF
            R10 = R8 & 0xFF;
            //ROM:01DB9524 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB9528 4                       AND     R10, R0, R8,LSR#8
            R10 = R0 & (R8 >> 8);
            //ROM:01DB952C 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB9530 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB9534 4                       AND     R10, R0, R8,LSR#16
            R10 = R0 & (R8 >> 16);
            //ROM:01DB9538 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB953C 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB9540 4                       LDR     R8, [R3,R8,LSL#2]
            R8 = get_long((R3 + (R8 << 2)));
            //ROM:01DB9544 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB9548 4                       EOR     R8, R10, R8
            R8 = R10 ^ R8;
            //ROM:01DB954C 4                       ADD     R7, R7, R8
            R7 = R7 + R8;
            //ROM:01DB9550 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9554 4                       AND     R10, R7, #0xFF
            R10 = R7 & 0xFF;
            //ROM:01DB9558 4                       LDR     R10, [R11,R10,LSL#2]
            R10 = get_long((R11 + (R10 << 2)));
            //ROM:01DB955C 4                       AND     R11, R0, R7,LSR#8
            R11 = R0 & (R7 >> 8);
            //ROM:01DB9560 4                       LDR     R11, [LR,R11,LSL#2]
            R11 = get_long((LR + (R11 << 2)));
            //ROM:01DB9564 4                       EOR     R11, R10, R11
            R11 = R10 ^ R11;
            //ROM:01DB9568 4                       AND     R10, R0, R7,LSR#16
            R10 = R0 & (R7 >> 16);
            //ROM:01DB956C 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB9570 4                       MOV     R7, R7,LSR#24
            R7 = (R7 >> 24);
            //ROM:01DB9574 4                       LDR     R7, [R3,R7,LSL#2]
            R7 = get_long((R3 + (R7 << 2)));
            //ROM:01DB9578 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB957C 4                       EOR     R10, R10, R7
            R10 = R10 ^ R7;
            //ROM:01DB9580 4                       ADD     R7, R8, R10
            R7 = R8 + R10;
            //ROM:01DB9584 4                       EOR     R7, R6, R7
            R7 = R6 ^ R7;
            //ROM:01DB9588 4                       EOR     R6, R5, R10
            R6 = R5 ^ R10;
            //ROM:01DB958C 4                       LDR     R5, [R2,#0x70]
            R5 = get_long((R2 + 0x70));
            //ROM:01DB9590 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB9594 4                       EOR     R8, R5, R7
            R8 = R5 ^ R7;
            //ROM:01DB9598 4                       LDR     R5, [R2,#0x74]
            R5 = get_long((R2 + 0x74));
            //ROM:01DB959C 4                       EOR     R5, R5, R6
            R5 = R5 ^ R6;
            //ROM:01DB95A0 4                       EOR     R5, R5, R8
            R5 = R5 ^ R8;
            //ROM:01DB95A4 4                       AND     R10, R5, #0xFF
            R10 = R5 & 0xFF;
            //ROM:01DB95A8 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB95AC 4                       AND     R10, R0, R5,LSR#8
            R10 = R0 & (R5 >> 8);
            //ROM:01DB95B0 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB95B4 4                       EOR     R11, R11, R10
            R11 = R11 ^ R10;
            //ROM:01DB95B8 4                       AND     R10, R0, R5,LSR#16
            R10 = R0 & (R5 >> 16);
            //ROM:01DB95BC 4                       LDR     R10, [R12,R10,LSL#2]
            R10 = get_long((R12 + (R10 << 2)));
            //ROM:01DB95C0 4                       MOV     R5, R5,LSR#24
            R5 = (R5 >> 24);
            //ROM:01DB95C4 4                       LDR     R5, [R3,R5,LSL#2]
            R5 = get_long((R3 + (R5 << 2)));
            //ROM:01DB95C8 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB95CC 4                       EOR     R5, R10, R5
            R5 = R10 ^ R5;
            //ROM:01DB95D0 4                       ADD     R8, R8, R5
            R8 = R8 + R5;
            //ROM:01DB95D4 4                       LDR     R11, =unk_2247E1C
            R11 = 0x2247E1C;
            //ROM:01DB95D8 4                       AND     R10, R8, #0xFF
            R10 = R8 & 0xFF;
            //ROM:01DB95DC 4                       LDR     R11, [R11,R10,LSL#2]
            R11 = get_long((R11 + (R10 << 2)));
            //ROM:01DB95E0 4                       AND     R10, R0, R8,LSR#8
            R10 = R0 & (R8 >> 8);
            //ROM:01DB95E4 4                       LDR     R10, [LR,R10,LSL#2]
            R10 = get_long((LR + (R10 << 2)));
            //ROM:01DB95E8 4                       EOR     R10, R11, R10
            R10 = R11 ^ R10;
            //ROM:01DB95EC 4                       AND     R11, R0, R8,LSR#16
            R11 = R0 & (R8 >> 16);
            //ROM:01DB95F0 4                       LDR     R11, [R12,R11,LSL#2]
            R11 = get_long((R12 + (R11 << 2)));
            //ROM:01DB95F4 4                       MOV     R8, R8,LSR#24
            R8 = (R8 >> 24);
            //ROM:01DB95F8 4                       LDR     R8, [R3,R8,LSL#2]
            R8 = get_long((R3 + (R8 << 2)));
            //ROM:01DB95FC 4                       EOR     R10, R10, R11
            R10 = R10 ^ R11;
            //ROM:01DB9600 4                       EOR     R8, R10, R8
            R8 = R10 ^ R8;
            //ROM:01DB9604 4                       ADD     R5, R5, R8
            R5 = R5 + R8;
            //ROM:01DB9608 4                       LDR     R10, =unk_2247E1C
            R10 = 0x2247E1C;
            //ROM:01DB960C 4                       MOV     R0, #0xFF
            R0 = 0xFF;
            //ROM:01DB9610 4                       AND     R0, R0, R5,LSR#8
            R0 = R0 & (R5 >> 8);
            //ROM:01DB9614 4                       AND     R11, R5, #0xFF
            R11 = R5 & 0xFF;
            //ROM:01DB9618 4                       LDR     R11, [R10,R11,LSL#2]
            R11 = get_long((R10 + (R11 << 2)));
            //ROM:01DB961C 4                       LDR     R0, [LR,R0,LSL#2]
            R0 = get_long((LR + (R0 << 2)));
            //ROM:01DB9620 4                       EOR     R11, R11, R0
            R11 = R11 ^ R0;
            //ROM:01DB9624 4                       MOV     R0, #0xFF
            R0 = 0xFF;
            //ROM:01DB9628 4                       AND     R0, R0, R5,LSR#16
            R0 = R0 & (R5 >> 16);
            //ROM:01DB962C 4                       LDR     R0, [R12,R0,LSL#2]
            R0 = get_long((R12 + (R0 << 2)));
            //ROM:01DB9630 4                       MOV     R5, R5,LSR#24
            R5 = (R5 >> 24);
            //ROM:01DB9634 4                       LDR     R5, [R3,R5,LSL#2]
            R5 = get_long((R3 + (R5 << 2)));
            //ROM:01DB9638 4                       EOR     R0, R11, R0
            R0 = R11 ^ R0;
            //ROM:01DB963C 4                       EOR     R0, R0, R5
            R0 = R0 ^ R5;
            //ROM:01DB9640 4                       ADD     R5, R8, R0
            R5 = R8 + R0;
            //ROM:01DB9644 4                       EOR     R4, R4, R5
            R4 = R4 ^ R5;
            //ROM:01DB9648 4                       LDR     R5, [R2,#0x78]
            R5 = get_long((R2 + 0x78));
            //ROM:01DB964C 4                       LDR     R2, [R2,#0x7C]
            R2 = get_long((R2 + 0x7C));
            //ROM:01DB9650 4                       EOR     R0, R9, R0
            R0 = R9 ^ R0;
            //ROM:01DB9654 4                       EOR     R2, R2, R0
            R2 = R2 ^ R0;
            //ROM:01DB9658 4                       EOR     R8, R5, R4
            R8 = R5 ^ R4;
            //ROM:01DB965C 4                       EOR     R2, R2, R8
            R2 = R2 ^ R8;
            //ROM:01DB9660 4                       AND     R5, R2, #0xFF
            R5 = R2 & 0xFF;
            //ROM:01DB9664 4                       LDR     R9, [R10,R5,LSL#2]
            R9 = get_long((R10 + (R5 << 2)));
            //ROM:01DB9668 4                       MOV     R5, #0xFF
            R5 = 0xFF;
            //ROM:01DB966C 4                       AND     R5, R5, R2,LSR#8
            R5 = R5 & (R2 >> 8);
            //ROM:01DB9670 4                       LDR     R5, [LR,R5,LSL#2]
            R5 = get_long((LR + (R5 << 2)));
            //ROM:01DB9674 4                       EOR     R9, R9, R5
            R9 = R9 ^ R5;
            //ROM:01DB9678 4                       MOV     R5, #0xFF
            R5 = 0xFF;
            //ROM:01DB967C 4                       AND     R5, R5, R2,LSR#16
            R5 = R5 & (R2 >> 16);
            //ROM:01DB9680 4                       LDR     R5, [R12,R5,LSL#2]
            R5 = get_long((R12 + (R5 << 2)));
            //ROM:01DB9684 4                       MOV     R2, R2,LSR#24
            R2 = (R2 >> 24);
            //ROM:01DB9688 4                       LDR     R2, [R3,R2,LSL#2]
            R2 = get_long((R3 + (R2 << 2)));
            //ROM:01DB968C 4                       EOR     R5, R9, R5
            R5 = R9 ^ R5;
            //ROM:01DB9690 4                       EOR     R5, R5, R2
            R5 = R5 ^ R2;
            //ROM:01DB9694 4                       ADD     R2, R8, R5
            R2 = R8 + R5;
            //ROM:01DB9698 4                       AND     R8, R2, #0xFF
            R8 = R2 & 0xFF;
            //ROM:01DB969C 4                       LDR     R9, [R10,R8,LSL#2]
            R9 = get_long((R10 + (R8 << 2)));
            //ROM:01DB96A0 4                       MOV     R8, #0xFF
            R8 = 0xFF;
            //ROM:01DB96A4 4                       AND     R8, R8, R2,LSR#8
            R8 = R8 & (R2 >> 8);
            //ROM:01DB96A8 4                       LDR     R8, [LR,R8,LSL#2]
            R8 = get_long((LR + (R8 << 2)));
            //ROM:01DB96AC 4                       EOR     R9, R9, R8
            R9 = R9 ^ R8;
            //ROM:01DB96B0 4                       MOV     R8, #0xFF
            R8 = 0xFF;
            //ROM:01DB96B4 4                       AND     R8, R8, R2,LSR#16
            R8 = R8 & (R2 >> 16);
            //ROM:01DB96B8 4                       LDR     R8, [R12,R8,LSL#2]
            R8 = get_long((R12 + (R8 << 2)));
            //ROM:01DB96BC 4                       MOV     R2, R2,LSR#24
            R2 = (R2 >> 24);
            //ROM:01DB96C0 4                       LDR     R2, [R3,R2,LSL#2]
            R2 = get_long((R3 + (R2 << 2)));
            //ROM:01DB96C4 4                       EOR     R8, R9, R8
            R8 = R9 ^ R8;
            //ROM:01DB96C8 4                       EOR     R2, R8, R2
            R2 = R8 ^ R2;
            //ROM:01DB96CC 4                       ADD     R5, R5, R2
            R5 = R5 + R2;
            //ROM:01DB96D0 4                       AND     R8, R5, #0xFF
            R8 = R5 & 0xFF;
            //ROM:01DB96D4 4                       LDR     R9, [R10,R8,LSL#2]
            R9 = get_long((R10 + (R8 << 2)));
            //ROM:01DB96D8 4                       MOV     R8, #0xFF
            R8 = 0xFF;
            //ROM:01DB96DC 4                       AND     R8, R8, R5,LSR#8
            R8 = R8 & (R5 >> 8);
            //ROM:01DB96E0 4                       LDR     LR, [LR,R8,LSL#2]
            LR = get_long((LR + (R8 << 2)));
            //ROM:01DB96E4 4                       MOV     R8, #0xFF
            R8 = 0xFF;
            //ROM:01DB96E8 4                       AND     R8, R8, R5,LSR#16
            R8 = R8 & (R5 >> 16);
            //ROM:01DB96EC 4                       LDR     R12, [R12,R8,LSL#2]
            R12 = get_long((R12 + (R8 << 2)));
            //ROM:01DB96F0 4                       EOR     LR, R9, LR
            LR = R9 ^ LR;
            //ROM:01DB96F4 4                       EOR     R12, LR, R12
            R12 = LR ^ R12;
            //ROM:01DB96F8 4                       MOV     LR, R5,LSR#24
            LR = (R5 >> 24);
            //ROM:01DB96FC 4                       LDR     R3, [R3,LR,LSL#2]
            R3 = get_long((R3 + (LR << 2)));
            //ROM:01DB9700 4                       STR     R0, [R1,#0xC]
            put_long((R1 + 0xC), R0);
            //ROM:01DB9704 4                       EOR     R3, R12, R3
            R3 = R12 ^ R3;
            //ROM:01DB9708 4                       ADD     R2, R2, R3
            R2 = R2 + R3;
            //ROM:01DB970C 4                       EOR     R2, R7, R2
            R2 = R7 ^ R2;
            //ROM:01DB9710 4                       EOR     R3, R6, R3
            R3 = R6 ^ R3;
            //ROM:01DB9714 4                       STMIA   R1, {R2-R4}
            put_long(R1 + 0x0, R2);
            put_long(R1 + 0x4, R3);
            put_long(R1 + 0x8, R4);
            //ROM:01DB9718 4                       MOV     R0, #0
            R0 = 0;
            //ROM:01DB971C 4                       LDMFD   SP!, {R4-R11,PC}
            R4 = get_long(SP + 0x0);
            R5 = get_long(SP + 0x4);
            R6 = get_long(SP + 0x8);
            R7 = get_long(SP + 0xC);
            R8 = get_long(SP + 0x10);
            R9 = get_long(SP + 0x14);
            R10 = get_long(SP + 0x18);
            R11 = get_long(SP + 0x1C);
            PC = get_long(SP + 0x20);
            SP = SP + 0x24;

            //
        }
    }
}
