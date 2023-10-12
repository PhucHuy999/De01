using BLL;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class Form1 : Form
    {
        //bool _them;
        private readonly LopService _lopService = new LopService();
        private readonly SinhVienService _sinhvienService = new SinhVienService();
        private SinhVien selectedSinhvien;
        
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvDanhSach);
                var listLop = _lopService.GetAll();
                var listSinhVien = _sinhvienService.GetAll();
                FillLopCombobox(listLop);
                BindGrid(listSinhVien);
                dgvDanhSach.SelectionChanged += dgvDanhSach_SelectionChanged;
                _showHide(true);
                //_them = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        void _showHide(bool kt)
        {
            btnLuu.Enabled = !kt;//// mới chạy thì lưu và hủy cho nó mờ đi
            btnKhongLuu.Enabled = !kt;////
            btnThem.Enabled = kt;
            btnSua.Enabled = kt;
            btnXoa.Enabled = kt;
            btnThoat.Enabled = kt;

            txtMaSV.Enabled = !kt;
            txtHoTenSV.Enabled = !kt;
            dtNgaySinh.Enabled = !kt;
            cmbLopHoc.Enabled = !kt;
        }
        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle =
           DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void FillLopCombobox(List<Lop> listLop)
        {
            listLop.Insert(0, new Lop());
            this.cmbLopHoc.DataSource = listLop;
            this.cmbLopHoc.DisplayMember = "TenLop";
            this.cmbLopHoc.ValueMember = "MaLop";
        }
        private void BindGrid(List<SinhVien> listSinhVien)
        {
            dgvDanhSach.Rows.Clear();
            foreach (var item in listSinhVien)
            {
                int index = dgvDanhSach.Rows.Add();
                dgvDanhSach.Rows[index].Cells[0].Value = item.MaSV;
                dgvDanhSach.Rows[index].Cells[1].Value = item.HotenSV;
                dgvDanhSach.Rows[index].Cells[2].Value = item.NgaySinh;

                if (item.Lop != null)
                    dgvDanhSach.Rows[index].Cells[3].Value = item.Lop.TenLop;

            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            _reset();
            _showHide(false);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            _showHide(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvDanhSach.SelectedRows.Count > 0)
            {
                // Hiển thị hộp thoại xác nhận trước khi xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa Sinh Viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    string sinhvienId = dgvDanhSach.SelectedRows[0].Cells[0].Value.ToString();
                    _sinhvienService.Delete(sinhvienId);

                    // Sau khi xóa thành công, cập nhật DataGridView
                    var listSach = _sinhvienService.GetAll();
                    BindGrid(listSach);
                    // Hiển thị thông báo sau khi xóa sách thành công
                    MessageBox.Show("Sách đã được xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một Sách để xóa.");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (KiemTraThongTinNhap())
            {
                SinhVien sinhvien = new SinhVien
                {
                    MaSV = txtMaSV.Text,
                    HotenSV = txtHoTenSV.Text,
                    NgaySinh = dtNgaySinh.Value,
                    MaLop = cmbLopHoc.SelectedIndex.ToString(),

                };

                // Gọi phương thức InsertUpdate để thêm hoặc cập nhật Student
                _sinhvienService.InsertUpdate(sinhvien);

                // Sau khi thêm hoặc cập nhật thành công, gọi lại BindGrid để cập nhật DataGridView
                var listSinhVien = _sinhvienService.GetAll();
                BindGrid(listSinhVien);

                _showHide(true);
            }
            
        }

        private void btnKhongLuu_Click(object sender, EventArgs e)
        {
            //_them = false;
            _showHide(true);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận trước khi thoát
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận xóa", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                var listSach = _sinhvienService.GetAll();
                BindGrid(listSach);
            }
            else
            {
                var filteredSach = _sinhvienService.SearchSach(keyword);
                BindGrid(filteredSach);
            }
        }
        void _reset()// reset lai các text khi sử dụng chức năng thêm
        {
            txtMaSV.Text = string.Empty;
            txtHoTenSV.Text = string.Empty;
            dtNgaySinh.Value = DateTime.Now;
            cmbLopHoc.Text = string.Empty;
        }
        
        
        private void dgvDanhSach_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDanhSach.SelectedRows.Count > 0)
            {
                DataGridViewCell cell = dgvDanhSach.SelectedRows[0].Cells[0]; // Lấy ô cần kiểm tra

                if (cell.Value != null)
                {
                    string sinhvienId = cell.Value.ToString();
                    selectedSinhvien = _sinhvienService.GetItem(sinhvienId);

                    if (selectedSinhvien != null)
                    {
                        txtMaSV.Text = selectedSinhvien.MaSV;
                        txtHoTenSV.Text = selectedSinhvien.HotenSV;
                        dtNgaySinh.Text = selectedSinhvien.NgaySinh.ToString();
                        cmbLopHoc.SelectedValue = selectedSinhvien.MaLop;
                    }
                }
            }
        }
        bool KiemTraThongTinNhap()
        {
            //string dienthoai = txtDienThoai.Text;
            //long _ketqua;
            //char[] mangDIENTHOAI = dienthoai.ToCharArray();

            if (txtHoTenSV.Text == "")
            {
                MessageBox.Show("Hãy nhập Họ Tên", "Thông Báo");
                txtHoTen.Focus();
                return false;
            }

            if (txtMaSV.Text == "")
            {
                MessageBox.Show("Hãy nhập Mã Sinh Viên", "Thông Báo");
                txtMaSV.Focus();
                return false;
            }

            //if (!(long.TryParse(dienthoai, out _ketqua)))// Kiểm tra định dạng số điện thoại là số
            //{
            //    MessageBox.Show("Hãy nhập đúng định dạng điện thoại là số", "Thông Báo");
            //    txtDienThoai.Focus();
            //    return false;
            //}

            //if (mangDIENTHOAI.Length != 10)
            //{
            //    MessageBox.Show("Số điện thoại phải đúng đủ 10 số", "Thông Báo");
            //    txtDienThoai.Focus();
            //    return false;
            //}

           

            if (cmbLopHoc.Text == "")
            {
                MessageBox.Show("Hãy chọn Lớp Học", "Thông Báo");
                cmbLopHoc.Focus();
                return false;
            }


            return true;

        }
    }
}
